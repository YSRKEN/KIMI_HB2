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
