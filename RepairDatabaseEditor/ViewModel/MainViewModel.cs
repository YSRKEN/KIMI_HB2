using RepairDatabaseEditor.Model;
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
        /// 艦娘一覧
        /// </summary>
        public ObservableCollection<Kammusu> KammusuList { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainViewModel()
        {
            KammusuList = new ObservableCollection<Kammusu>();
            KammusuList.Add(new Kammusu() { Id = 0, Name = "○" });
            KammusuList.Add(new Kammusu() { Id = 1, Name = "吹雪" });
        }
    }
}
