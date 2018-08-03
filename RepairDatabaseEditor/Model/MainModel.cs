using Reactive.Bindings;
using RepairDatabaseEditor.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
        public ObservableCollection<Kammusu> KammusuList { get; }

        /// <summary>
        /// 選択中の艦娘
        /// </summary>
        public ReactiveProperty<Kammusu> SelectedKammusu { get; set; } = new ReactiveProperty<Kammusu>(new Kammusu());

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
        public ObservableCollection<Weapon> WeaponList { get; }

        /// <summary>
        /// 選択中の装備
        /// </summary>
        public ReactiveProperty<Weapon> SelectedWeapon { get; set; } = new ReactiveProperty<Weapon>(new Weapon());

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
            // リストボックスに標示するデータを読み込み
            this.dataStore = dataStore;
            KammusuList = new ObservableCollection<Kammusu>(dataStore.Kammusus);
            WeaponList = new ObservableCollection<Weapon>(dataStore.Weapons);

            // 選択変更時の処理を記述
            SelectedKammusu.Subscribe(value => {
                KammusuId.Value = value.Id.ToString();
                KammusuName.Value = value.Name;
            });
            SelectedWeapon.Subscribe(value => {
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
            MessageBox.Show("PostKammusuCommand");
        }

        /// <summary>
        /// 艦娘を更新
        /// </summary>
        public void PutKammusu()
        {
            MessageBox.Show("PutKammusuCommand");
        }

        /// <summary>
        /// 艦娘を削除
        /// </summary>
        public void DeleteKammusu()
        {
            MessageBox.Show("DeleteKammusuCommand");
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
