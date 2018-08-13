using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using RepairDatabaseEditor.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
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
        /// 改修の拡張情報(開発資材通常)(数字に変換後)
        /// </summary>
        private ReadOnlyReactiveProperty<int> extraInfoGearProb;

        /// <summary>
        /// 改修の拡張情報(開発資材確実)(数字に変換後)
        /// </summary>
        private ReadOnlyReactiveProperty<int> extraInfoGearSure;

        /// <summary>
        /// 改修の拡張情報(改修資材通常)(数字に変換後)
        /// </summary>
        private ReadOnlyReactiveProperty<int> extraInfoScrewProb;

        /// <summary>
        /// 改修の拡張情報(改修資材確実)(数字に変換後)
        /// </summary>
        private ReadOnlyReactiveProperty<int> extraInfoScrewSure;

        /// <summary>
        /// 改修の拡張情報(消費装備数)(数字に変換後)
        /// </summary>
        private ReadOnlyReactiveProperty<int> extraInfoLostCount;

        private ReadOnlyReactiveProperty<int> selectedRepairStep;

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
        /// 改修の拡張情報一覧
        /// </summary>
        public ReadOnlyReactiveCollection<ExtraInfo> ExtraInfoList { get; }

        /// <summary>
        /// 選択中の拡張情報
        /// </summary>
        public ReactiveProperty<ExtraInfo> SelectedExtraInfo { get; } = new ReactiveProperty<ExtraInfo>(new ExtraInfo());

        /// <summary>
        /// 選択中の装備2
        /// </summary>
        public ReactiveProperty<Weapon> SelectedWeapon3 { get; } = new ReactiveProperty<Weapon>(new Weapon());

        /// <summary>
        /// 選択中の装備3
        /// </summary>
        public ReactiveProperty<Weapon> SelectedWeapon4 { get; } = new ReactiveProperty<Weapon>(new Weapon());

        public ReadOnlyReactiveCollection<string> StepList { get; }

        /// <summary>
        /// 改修の基本情報(燃料)
        /// </summary>
        public ReactiveProperty<string> ExtraInfoGearProb { get; } = new ReactiveProperty<string>("");

        /// <summary>
        /// 改修の基本情報(弾薬)
        /// </summary>
        public ReactiveProperty<string> ExtraInfoGearSure { get; } = new ReactiveProperty<string>("");

        /// <summary>
        /// 改修の基本情報(鋼材)
        /// </summary>
        public ReactiveProperty<string> ExtraInfoScrewProb { get; } = new ReactiveProperty<string>("");

        /// <summary>
        /// 改修の基本情報(ボーキ)
        /// </summary>
        public ReactiveProperty<string> ExtraInfoScrewSure { get; } = new ReactiveProperty<string>("");

        /// <summary>
        /// 改修の拡張情報(改修段階)
        /// </summary>
        public ReactiveProperty<string> SelectedRepairStep { get; } = new ReactiveProperty<string>("");

        /// <summary>
        /// 選択中の装備5
        /// </summary>
        public ReactiveProperty<Weapon> SelectedWeapon5 { get; } = new ReactiveProperty<Weapon>(new Weapon());

        /// <summary>
        /// 改修の消費装備数
        /// </summary>
        public ReactiveProperty<string> ExtraInfoLostCount { get; } = new ReactiveProperty<string>("");

        /// <summary>
        /// 更新後装備を選択できるようにするか？
        /// </summary>
        public ReadOnlyReactiveProperty<bool> NextWeaponFlg { get; }

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
        /// 改修の拡張情報を追加
        /// </summary>
        public ReactiveCommand PostExtraInfoCommand { get; }

        /// <summary>
        /// 改修の拡張情報を更新
        /// </summary>
        public ReactiveCommand PutExtraInfoCommand { get; }

        /// <summary>
        /// 改修の拡張情報を削除
        /// </summary>
        public ReactiveCommand DeleteExtraInfoCommand { get; }

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
            ExtraInfoList = dataStore.ExtraInfoList.ToReadOnlyReactiveCollection();
            StepList = (new ObservableCollection<string>() { "★0～★5", "★6～★9", "★max" }).ToReadOnlyReactiveCollection();

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
            extraInfoGearProb = ExtraInfoGearProb.Select(str =>
            {
                int num = -1;
                return int.TryParse(str, out num) ? num : -1;
            }).ToReadOnlyReactiveProperty();
            extraInfoGearSure = ExtraInfoGearSure.Select(str =>
            {
                int num = -1;
                return int.TryParse(str, out num) ? num : -1;
            }).ToReadOnlyReactiveProperty();
            extraInfoScrewProb = ExtraInfoScrewProb.Select(str =>
            {
                int num = -1;
                return int.TryParse(str, out num) ? num : -1;
            }).ToReadOnlyReactiveProperty();
            extraInfoScrewSure = ExtraInfoScrewSure.Select(str =>
            {
                int num = -1;
                return int.TryParse(str, out num) ? num : -1;
            }).ToReadOnlyReactiveProperty();
            extraInfoLostCount = ExtraInfoLostCount.Select(str =>
            {
                int num = -1;
                return int.TryParse(str, out num) ? num : -1;
            }).ToReadOnlyReactiveProperty();
            NextWeaponFlg = SelectedRepairStep.Select(stepString => stepString == "★max").ToReadOnlyReactiveProperty();

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

            PostExtraInfoCommand = new[] {
                extraInfoGearProb.Select(num => num >= 0),
                extraInfoGearSure.Select(num => num >= 0),
                extraInfoScrewProb.Select(num => num >= 0),
                extraInfoScrewSure.Select(num => num >= 0),
                extraInfoLostCount.Select(num => num >= 0),
                SelectedWeapon3.Select(weapon => weapon != null && weapon.Name != null),
                SelectedWeapon4.Select(weapon => weapon != null && weapon.Name != null),
                SelectedWeapon5.Select(weapon => weapon != null && weapon.Name != null)
            }.CombineLatestValuesAreAllTrue().ToReactiveCommand();
            PutExtraInfoCommand = new[] {
                extraInfoGearProb.Select(num => num >= 0),
                extraInfoGearSure.Select(num => num >= 0),
                extraInfoScrewProb.Select(num => num >= 0),
                extraInfoScrewSure.Select(num => num >= 0),
                extraInfoLostCount.Select(num => num >= 0),
                SelectedWeapon3.Select(weapon => weapon != null && weapon.Name != null),
                SelectedWeapon4.Select(weapon => weapon != null && weapon.Name != null),
                SelectedWeapon5.Select(weapon => weapon != null && weapon.Name != null),
                SelectedExtraInfo.Select(extraInfo => extraInfo != null && extraInfo.WeaponName != null),
            }.CombineLatestValuesAreAllTrue().ToReactiveCommand();
            DeleteExtraInfoCommand = SelectedExtraInfo.Select(extraInfo => extraInfo != null && extraInfo.WeaponName != null)
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
            SelectedExtraInfo.Subscribe(value => {
                if (value == null)
                    return;
                SelectedWeapon3.Value = value.Id != 0 ? WeaponList.Where(w => w.Id == value.Id).First() : null;
                SelectedWeapon4.Value = value.Id != 0 ? WeaponList.Where(w => w.Id == value.NextId).First() : null;
                SelectedWeapon5.Value = value.Id != 0 ? WeaponList.Where(w => w.Id == value.LostId).First() : null;
                SelectedRepairStep.Value = value.StepName2;
                ExtraInfoGearProb.Value = value.GearProb.ToString();
                ExtraInfoGearSure.Value = value.GearSure.ToString();
                ExtraInfoScrewProb.Value = value.ScrewProb.ToString();
                ExtraInfoScrewSure.Value = value.ScrewSure.ToString();
                ExtraInfoLostCount.Value = value.LostCount.ToString();
            });
            selectedRepairStep = SelectedRepairStep.Select(stepString =>
            {
                switch (stepString)
                {
                    case "★0～★5":
                        return 0;
                    case "★6～★9":
                        return 6;
                    case "★max":
                        return 10;
                    default:
                        return 0;
                }
            }).ToReadOnlyReactiveProperty();
            SelectedRepairStep.Subscribe(value => {
                return;
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
            PostExtraInfoCommand.Subscribe(PostExtraInfo);
            PutExtraInfoCommand.Subscribe(PutExtraInfo);
            DeleteExtraInfoCommand.Subscribe(DeleteExtraInfo);
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

        /// <summary>
        /// 拡張情報を追加
        /// </summary>
        public void PostExtraInfo()
        {
            // 追加操作を行う
            if (dataStore.PostWeaponExtraInfo(SelectedWeapon3.Value.Id, selectedRepairStep.Value,
                NextWeaponFlg.Value ? SelectedWeapon4.Value.Id : 0,
                extraInfoGearProb.Value, extraInfoGearSure.Value, extraInfoScrewProb.Value, extraInfoScrewSure.Value,
                SelectedWeapon5.Value.Id, extraInfoLostCount.Value))
            {
                MessageBox.Show("改修の拡張情報データを追加しました。", "改修情報DBエディタ", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("改修の拡張情報データを追加できませんでした。", "改修情報DBエディタ", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// 拡張情報を変更
        /// </summary>
        public void PutExtraInfo()
        {
            // 更新操作を行う
            if (dataStore.PutWeaponExtraInfo(SelectedWeapon3.Value.Id, selectedRepairStep.Value,
                NextWeaponFlg.Value ? SelectedWeapon4.Value.Id : 0,
                extraInfoGearProb.Value, extraInfoGearSure.Value, extraInfoScrewProb.Value, extraInfoScrewSure.Value,
                SelectedWeapon5.Value.Id, extraInfoLostCount.Value, SelectedExtraInfo.Value.Id))
            {
                MessageBox.Show("改修の拡張情報データを更新しました。", "改修情報DBエディタ", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("改修の拡張情報データを更新できませんでした。", "改修情報DBエディタ", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// 拡張情報を削除
        /// </summary>
        public void DeleteExtraInfo()
        {
            // 削除操作を行う
            if (dataStore.DeleteWeaponExtraInfo(SelectedExtraInfo.Value.Id))
            {
                MessageBox.Show("改修の拡張情報データを削除しました。", "改修情報DBエディタ", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("改修の拡張情報データを削除できませんでした。", "改修情報DBエディタ", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
