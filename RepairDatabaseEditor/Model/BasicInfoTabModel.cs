using Reactive.Bindings;
using RepairDatabaseEditor.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace RepairDatabaseEditor.Model
{
    class BasicInfoTabModel
    {
        /// <summary>
        /// データベース
        /// </summary>
        private DataStore dataStore;

        /// <summary>
        /// 艦娘の番号(数字に変換後)
        /// </summary>
        private ReadOnlyReactiveProperty<int> kammusuId;

        /// <summary>
        /// 艦娘一覧
        /// </summary>
        public ReadOnlyReactiveCollection<Kammusu> KammusuList { get; }

        /// <summary>
        /// 選択中の艦娘
        /// </summary>
        public ReactiveProperty<Kammusu> SelectedKammusu { get; } = new ReactiveProperty<Kammusu>(new Kammusu());

        /// <summary>
        /// 艦娘の番号
        /// </summary>
        public ReactiveProperty<string> KammusuId { get; } = new ReactiveProperty<string>("");

        /// <summary>
        /// 艦娘の名前
        /// </summary>
        public ReactiveProperty<string> KammusuName { get; } = new ReactiveProperty<string>("");

        /// <summary>
        /// 艦娘を追加
        /// </summary>
        public ReactiveCommand PostKammusuCommand { get; }

        /// <summary>
        /// 艦娘を更新
        /// </summary>
        public ReactiveCommand PutKammusuCommand { get; }

        /// <summary>
        /// 艦娘を削除
        /// </summary>
        public ReactiveCommand DeleteKammusuCommand { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataStore">DIするデータベース</param>
        public BasicInfoTabModel(DataStore dataStore)
        {
            // リストボックスに表示するデータを読み込み
            this.dataStore = dataStore;
            KammusuList = dataStore.KammusuList.ToReadOnlyReactiveCollection();

            // リストボックスの表示を常にソートするようにする
            {
                var collectionView = CollectionViewSource.GetDefaultView(this.KammusuList);
                collectionView.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Ascending));
            }

            // オブジェクトからオブジェクトを構成
            kammusuId = KammusuId.Select(str =>
            {
                int num = -1;
                return int.TryParse(str, out num) ? num : -1;
            }).ToReadOnlyReactiveProperty();

            PostKammusuCommand = kammusuId.Select(num => num >= 0)
                .CombineLatest(KammusuName, (flg, str) => flg && str != "")
                .ToReactiveCommand();
            PutKammusuCommand = kammusuId.Select(num => num >= 0)
                .CombineLatest(KammusuName, (flg, str) => flg && str != "")
                .CombineLatest(SelectedKammusu, (flg, kammusu) => flg && kammusu != null && kammusu.Name != null)
                .ToReactiveCommand();
            DeleteKammusuCommand = SelectedKammusu.Select(kammusu => kammusu != null && kammusu.Name != null)
                .ToReactiveCommand();

            // 選択変更時の処理を記述
            SelectedKammusu.Subscribe(value => {
                if (value == null)
                    return;
                KammusuId.Value = value.Id.ToString();
                KammusuName.Value = value.Name;
            });

            // ボタンを押した際の処理を記述
            PostKammusuCommand.Subscribe(PostKammusu);
            PutKammusuCommand.Subscribe(PutKammusu);
            DeleteKammusuCommand.Subscribe(DeleteKammusu);
        }

        /// <summary>
        /// 艦娘を追加
        /// </summary>
        public void PostKammusu()
        {
            // 追加操作を行う
            if (dataStore.PostKammusu(kammusuId.Value, KammusuName.Value))
            {
                MessageBox.Show("艦娘データを追加しました。", "改修情報DBエディタ", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("艦娘データを追加できませんでした。", "改修情報DBエディタ", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// 艦娘を更新
        /// </summary>
        public void PutKammusu()
        {
            // 更新操作を行う
            if (dataStore.PutKammusu(kammusuId.Value, KammusuName.Value, SelectedKammusu.Value.Id))
            {
                MessageBox.Show("艦娘データを更新しました。", "改修情報DBエディタ", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("艦娘データを更新できませんでした。", "改修情報DBエディタ", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// 艦娘を削除
        /// </summary>
        public void DeleteKammusu()
        {
            // 削除操作を行う
            if (dataStore.DeleteKammusu(SelectedKammusu.Value.Id))
            {
                MessageBox.Show("艦娘データを削除しました。", "改修情報DBエディタ", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("艦娘データを削除できませんでした。", "改修情報DBエディタ", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}