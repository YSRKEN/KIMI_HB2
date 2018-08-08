using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using RepairDatabaseEditor.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
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
        /// 艦娘の番号(数字に変換後)
        /// </summary>
        private ReadOnlyReactiveProperty<int> kammusuId;

        /// <summary>
        /// 装備の番号(数字に変換後)
        /// </summary>
        private ReadOnlyReactiveProperty<int> weaponId;

        /// <summary>
        /// 改修の基本情報(燃料)(数字に変換後)
        /// </summary>
        private ReadOnlyReactiveProperty<int> basicInfoFuel;

        /// <summary>
        /// 改修の基本情報(弾薬)(数字に変換後)
        /// </summary>
        private ReadOnlyReactiveProperty<int> basicInfoAmmo;

        /// <summary>
        /// 改修の基本情報(鋼材)(数字に変換後)
        /// </summary>
        private ReadOnlyReactiveProperty<int> basicInfoSteel;

        /// <summary>
        /// 改修の基本情報(ボーキ)(数字に変換後)
        /// </summary>
        private ReadOnlyReactiveProperty<int> basicInfoBauxite;

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
        /// 改修の基本情報一覧
        /// </summary>
        public ReadOnlyReactiveCollection<BasicInfo> BasicInfoList { get; }

        /// <summary>
        /// 選択中の基本情報
        /// </summary>
        public ReactiveProperty<BasicInfo> SelectedBasicInfo { get; } = new ReactiveProperty<BasicInfo>(new BasicInfo());

        /// <summary>
        /// 選択中の装備2
        /// </summary>
        public ReactiveProperty<Weapon> SelectedWeapon2 { get; } = new ReactiveProperty<Weapon>(new Weapon());

        /// <summary>
        /// 改修の基本情報(燃料)
        /// </summary>
        public ReactiveProperty<string> BasicInfoFuel { get; } = new ReactiveProperty<string>("");

        /// <summary>
        /// 改修の基本情報(弾薬)
        /// </summary>
        public ReactiveProperty<string> BasicInfoAmmo { get; } = new ReactiveProperty<string>("");

        /// <summary>
        /// 改修の基本情報(鋼材)
        /// </summary>
        public ReactiveProperty<string> BasicInfoSteel { get; } = new ReactiveProperty<string>("");

        /// <summary>
        /// 改修の基本情報(ボーキ)
        /// </summary>
        public ReactiveProperty<string> BasicInfoBauxite { get; } = new ReactiveProperty<string>("");

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
        /// 装備を追加
        /// </summary>
        public ReactiveCommand PostWeaponCommand { get; }

        /// <summary>
        /// 装備を更新
        /// </summary>
        public ReactiveCommand PutWeaponCommand { get; }

        /// <summary>
        /// 装備を削除
        /// </summary>
        public ReactiveCommand DeleteWeaponCommand { get; }

        /// <summary>
        /// 改修の基本情報を追加
        /// </summary>
        public ReactiveCommand PostBasicInfoCommand { get; }

        /// <summary>
        /// 改修の基本情報を更新
        /// </summary>
        public ReactiveCommand PutBasicInfoCommand { get; }

        /// <summary>
        /// 改修の基本情報を削除
        /// </summary>
        public ReactiveCommand DeleteBasicInfoCommand { get; }

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
            BasicInfoList = dataStore.BasicInfoList.ToReadOnlyReactiveCollection();

            // リストボックスの表示を常にソートするようにする
            {
                var collectionView = CollectionViewSource.GetDefaultView(this.KammusuList);
                collectionView.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Ascending));
            }
            {
                var collectionView = CollectionViewSource.GetDefaultView(this.WeaponList);
                collectionView.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Ascending));
            }
            {
                var collectionView = CollectionViewSource.GetDefaultView(this.BasicInfoList);
                collectionView.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Ascending));
            }

            // オブジェクトからオブジェクトを構成
            kammusuId = KammusuId.Select(str =>
            {
                int num = -1;
                return int.TryParse(str, out num) ? num : -1;
            }).ToReadOnlyReactiveProperty();
            weaponId = WeaponId.Select(str =>
            {
                int num = -1;
                return int.TryParse(str, out num) ? num : -1;
            }).ToReadOnlyReactiveProperty();
            basicInfoFuel = BasicInfoFuel.Select(str =>
            {
                int num = -1;
                return int.TryParse(str, out num) ? num : -1;
            }).ToReadOnlyReactiveProperty();
            basicInfoAmmo = BasicInfoAmmo.Select(str =>
            {
                int num = -1;
                return int.TryParse(str, out num) ? num : -1;
            }).ToReadOnlyReactiveProperty();
            basicInfoSteel = BasicInfoSteel.Select(str =>
            {
                int num = -1;
                return int.TryParse(str, out num) ? num : -1;
            }).ToReadOnlyReactiveProperty();
            basicInfoBauxite = BasicInfoBauxite.Select(str =>
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

            PostWeaponCommand = weaponId.Select(num => num >= 0)
                .CombineLatest(WeaponName, (flg, str) => flg && str != "")
                .ToReactiveCommand();
            PutWeaponCommand = weaponId.Select(num => num >= 0)
                .CombineLatest(WeaponName, (flg, str) => flg && str != "")
                .CombineLatest(SelectedWeapon, (flg, weapon) => flg && weapon != null && weapon.Name != null)
                .ToReactiveCommand();
            DeleteWeaponCommand = SelectedWeapon.Select(weapon => weapon != null && weapon.Name != null)
                .ToReactiveCommand();

            PostBasicInfoCommand = new[] {
                basicInfoFuel.Select(num => num >= 0),
                basicInfoAmmo.Select(num => num >= 0),
                basicInfoFuel.Select(num => num >= 0),
                basicInfoBauxite.Select(num => num >= 0),
                SelectedWeapon2.Select(weapon => weapon != null && weapon.Name != null)
            }.CombineLatestValuesAreAllTrue().ToReactiveCommand();
            PutBasicInfoCommand = new[] {
                basicInfoFuel.Select(num => num >= 0),
                basicInfoAmmo.Select(num => num >= 0),
                basicInfoFuel.Select(num => num >= 0),
                basicInfoBauxite.Select(num => num >= 0),
                SelectedBasicInfo.Select(basicInfo => basicInfo != null && basicInfo.Name != null),
                SelectedWeapon2.Select(weapon => weapon != null && weapon.Name != null)
            }.CombineLatestValuesAreAllTrue().ToReactiveCommand();
            DeleteBasicInfoCommand = SelectedBasicInfo.Select(basicInfo => basicInfo != null && basicInfo.Name != null)
                .ToReactiveCommand();

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
            SelectedBasicInfo.Subscribe(value => {
                if (value == null)
                    return;
                SelectedWeapon2.Value = value.Id != 0 ? WeaponList.Where(w => w.Id == value.Id).First() : null;
                BasicInfoFuel.Value = value.Fuel.ToString();
                BasicInfoAmmo.Value = value.Ammo.ToString();
                BasicInfoSteel.Value = value.Steel.ToString();
                BasicInfoBauxite.Value = value.Bauxite.ToString();
            });

            // ボタンを押した際の処理を記述
            PostKammusuCommand.Subscribe(PostKammusu);
            PutKammusuCommand.Subscribe(PutKammusu);
            DeleteKammusuCommand.Subscribe(DeleteKammusu);
            PostWeaponCommand.Subscribe(PostWeapon);
            PutWeaponCommand.Subscribe(PutWeapon);
            DeleteWeaponCommand.Subscribe(DeleteWeapon);
            PostBasicInfoCommand.Subscribe(PostBasicInfo);
            PutBasicInfoCommand.Subscribe(PutBasicInfo);
            DeleteBasicInfoCommand.Subscribe(DeleteBasicInfo);
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

        /// <summary>
        /// 装備を追加
        /// </summary>
        public void PostWeapon()
        {
            // 追加操作を行う
            if (dataStore.PostWeapon(weaponId.Value, WeaponName.Value))
            {
                MessageBox.Show("装備データを追加しました。", "改修情報DBエディタ", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("装備データを追加できませんでした。", "改修情報DBエディタ", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// 装備を更新
        /// </summary>
        public void PutWeapon()
        {
            // 更新操作を行う
            if (dataStore.PutWeapon(weaponId.Value, WeaponName.Value, SelectedWeapon.Value.Id))
            {
                MessageBox.Show("装備データを更新しました。", "改修情報DBエディタ", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("装備データを更新できませんでした。", "改修情報DBエディタ", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// 装備を削除
        /// </summary>
        public void DeleteWeapon()
        {
            // 削除操作を行う
            if (dataStore.DeleteWeapon(SelectedWeapon.Value.Id))
            {
                MessageBox.Show("装備データを削除しました。", "改修情報DBエディタ", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("装備データを削除できませんでした。", "改修情報DBエディタ", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// 基本情報を追加
        /// </summary>
        public void PostBasicInfo()
        {
            // 追加操作を行う
            if (dataStore.PostWeaponBasicInfo(
                SelectedWeapon2.Value.Id, basicInfoFuel.Value, basicInfoAmmo.Value,
                basicInfoSteel.Value, basicInfoBauxite.Value))
            {
                MessageBox.Show("改修の基本情報データを追加しました。", "改修情報DBエディタ", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("改修の基本情報データを追加できませんでした。", "改修情報DBエディタ", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// 基本情報を変更
        /// </summary>
        public void PutBasicInfo()
        {
            // 更新操作を行う
            if (dataStore.PutWeaponBasicInfo(
                SelectedWeapon2.Value.Id, basicInfoFuel.Value, basicInfoAmmo.Value,
                basicInfoSteel.Value, basicInfoBauxite.Value, SelectedBasicInfo.Value.Id))
            {
                MessageBox.Show("改修の基本情報データを更新しました。", "改修情報DBエディタ", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("改修の基本情報データを更新できませんでした。", "改修情報DBエディタ", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// 基本情報を削除
        /// </summary>
        public void DeleteBasicInfo()
        {
            // 削除操作を行う
            if (dataStore.DeleteWeaponBasicInfo(SelectedBasicInfo.Value.Id))
            {
                MessageBox.Show("改修の基本情報データを削除しました。", "改修情報DBエディタ", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("改修の基本情報データを削除できませんでした。", "改修情報DBエディタ", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
