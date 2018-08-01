using RepairDatabaseEditor.Model;
using RepairDatabaseEditor.Service;
using System.Collections.ObjectModel;
using System.ComponentModel;


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
        public ObservableCollection<Kammusu> KammusuList { get; set; }

        /// <summary>
        /// 装備一覧
        /// </summary>
        public ObservableCollection<Weapon> WeaponList { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainViewModel(DataStore dataStore)
        {
            this.dataStore = dataStore;
            KammusuList = new ObservableCollection<Kammusu>(dataStore.Kammusus);
            WeaponList = new ObservableCollection<Weapon>(dataStore.Weapons);
        }
    }
}
