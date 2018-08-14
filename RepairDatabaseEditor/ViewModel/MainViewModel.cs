using RepairDatabaseEditor.Model;
using System.ComponentModel;

namespace RepairDatabaseEditor.ViewModel
{
    /// <summary>
    /// メイン画面のViewModel
    /// </summary>
    class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public MainModel model { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainViewModel(MainModel model)
        {
            this.model = model;
        }
    }
}
