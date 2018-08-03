using Reactive.Bindings;
using RepairDatabaseEditor.Model;
using RepairDatabaseEditor.Service;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System;

namespace RepairDatabaseEditor.ViewModel
{
    /// <summary>
    /// メイン画面のViewModel
    /// </summary>
    class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// データベース
        /// </summary>
        private DataStore dataStore;

        /// <summary>
        /// 艦娘一覧
        /// </summary>
        public ObservableCollection<Kammusu> KammusuList { get; }

        /// <summary>
        /// 装備一覧
        /// </summary>
        public ObservableCollection<Weapon> WeaponList { get; }

        /// <summary>
        /// 選択中の艦娘の名前
        /// </summary>
        public ReactiveProperty<Kammusu> SelectedKammusu { get; set; } = new ReactiveProperty<Kammusu>(new Kammusu());

        /// <summary>
        /// 選択中の装備の名前
        /// </summary>
        public ReactiveProperty<Weapon> SelectedWeapon { get; set; } = new ReactiveProperty<Weapon>(new Weapon());

        /// <summary>
        /// 艦娘の名前
        /// </summary>
        public ReactiveProperty<string> KammusuName { get; } = new ReactiveProperty<string>("");

        /// <summary>
        /// 装備の名前
        /// </summary>
        public ReactiveProperty<string> WeaponName { get; } = new ReactiveProperty<string>("");

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainViewModel(DataStore dataStore)
        {
            // リストボックスに標示するデータを読み込み
            this.dataStore = dataStore;
            KammusuList = new ObservableCollection<Kammusu>(dataStore.Kammusus);
            WeaponList = new ObservableCollection<Weapon>(dataStore.Weapons);

            // 選択変更時の処理を記述
            SelectedKammusu.Subscribe(value => {
                KammusuName.Value = value.Name;
            });
            SelectedWeapon.Subscribe(value => {
                WeaponName.Value = value.Name;
            });
        }
    }
}
