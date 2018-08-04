using Reactive.Bindings;
using RepairDatabaseEditor.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace RepairDatabaseEditor.Model
{
    class MainModel
    {
        /// <summary>
        /// データベース
        /// </summary>
        private DataStore dataStore;

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
        /// 装備一覧
        /// </summary>
        public ReadOnlyReactiveCollection<Weapon> WeaponList { get; }

        /// <summary>
        /// 選択中の装備
        /// </summary>
        public ReactiveProperty<Weapon> SelectedWeapon { get; } = new ReactiveProperty<Weapon>(new Weapon());

        /// <summary>
        /// 装備の番号
        /// </summary>
        public ReactiveProperty<string> WeaponId { get; } = new ReactiveProperty<string>("");

        /// <summary>
        /// 装備の名前
        /// </summary>
        public ReactiveProperty<string> WeaponName { get; } = new ReactiveProperty<string>("");

        /// <summary>
        /// 艦娘を追加
        /// </summary>
        public ReactiveCommand PostKammusuCommand { get; } = new ReactiveCommand();

        /// <summary>
        /// 艦娘を更新
        /// </summary>
        public ReactiveCommand PutKammusuCommand { get; } = new ReactiveCommand();

        /// <summary>
        /// 艦娘を削除
        /// </summary>
        public ReactiveCommand DeleteKammusuCommand { get; } = new ReactiveCommand();

        /// <summary>
        /// 装備を追加
        /// </summary>
        public ReactiveCommand PostWeaponCommand { get; } = new ReactiveCommand();

        /// <summary>
        /// 装備を更新
        /// </summary>
        public ReactiveCommand PutWeaponCommand { get; } = new ReactiveCommand();

        /// <summary>
        /// 装備を削除
        /// </summary>
        public ReactiveCommand DeleteWeaponCommand { get; } = new ReactiveCommand();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataStore">DIするデータベース</param>
        public MainModel(DataStore dataStore)
        {
            // リストボックスに表示するデータを読み込み
            this.dataStore = dataStore;
            KammusuList = dataStore.KammusuList.ToReadOnlyReactiveCollection();
            WeaponList = dataStore.WeaponList.ToReadOnlyReactiveCollection();

            // リストボックスの表示を常にソートするようにする
            {
                var collectionView = CollectionViewSource.GetDefaultView(this.KammusuList);
                collectionView.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Ascending));
            }
            {
                var collectionView = CollectionViewSource.GetDefaultView(this.WeaponList);
                collectionView.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Ascending));
            }

            // 選択変更時の処理を記述
            SelectedKammusu.Subscribe(value => {
                if (value == null)
                    return;
                KammusuId.Value = value.Id.ToString();
                KammusuName.Value = value.Name;
            });
            SelectedWeapon.Subscribe(value => {
                if (value == null)
                    return;
                WeaponId.Value = value.Id.ToString();
                WeaponName.Value = value.Name;
            });

            // ボタンを押した際の処理を記述
            PostKammusuCommand.Subscribe(PostKammusu);
            PutKammusuCommand.Subscribe(PutKammusu);
            DeleteKammusuCommand.Subscribe(DeleteKammusu);
            PostWeaponCommand.Subscribe(PostWeapon);
            PutWeaponCommand.Subscribe(PutWeapon);
            DeleteWeaponCommand.Subscribe(DeleteWeapon);
        }

        /// <summary>
        /// 艦娘を追加
        /// </summary>
        public void PostKammusu()
        {
            // 艦番がパースできない際は何もしない
            int kammusuId = -1;
            if(!int.TryParse(KammusuId.Value, out kammusuId))
            {
                return;
            }

            // 艦名が空白な際は何もしない
            if(KammusuName.Value == "")
            {
                return;
            }

            // 追加操作を行う
            if (dataStore.PostKammusu(kammusuId, KammusuName.Value))
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
            // 艦番がパースできない際は何もしない
            int kammusuId = -1;
            if (!int.TryParse(KammusuId.Value, out kammusuId))
            {
                return;
            }

            // 艦名が空白な際は何もしない
            if (KammusuName.Value == "")
            {
                return;
            }

            // そもそも選択していない際は何もしない
            if (SelectedKammusu.Value.Name == null)
            {
                return;
            }

            // 更新操作を行う
            if (dataStore.PutKammusu(kammusuId, KammusuName.Value, SelectedKammusu.Value.Id))
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
            // そもそも選択していない際は何もしない
            if (SelectedKammusu.Value.Name == null)
            {
                return;
            }

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

        /// <summary>
        /// 装備を追加
        /// </summary>
        public void PostWeapon()
        {
            MessageBox.Show("PostKammusuCommand");
        }

        /// <summary>
        /// 装備を更新
        /// </summary>
        public void PutWeapon()
        {
            MessageBox.Show("PutKammusuCommand");
        }

        /// <summary>
        /// 装備を削除
        /// </summary>
        public void DeleteWeapon()
        {
            MessageBox.Show("DeleteKammusuCommand");
        }
    }
}
