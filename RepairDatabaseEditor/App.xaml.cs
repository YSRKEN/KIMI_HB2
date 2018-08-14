using RepairDatabaseEditor.Model;
using RepairDatabaseEditor.Service;
using RepairDatabaseEditor.View;
using RepairDatabaseEditor.ViewModel;
using System.Windows;

namespace RepairDatabaseEditor
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var dataStore = new DataStore();
            var mv = new MainView();
            var model = new MainModel(dataStore);
            var bitModel = new BasicInfoTabModel(dataStore);
            var mvm = new MainViewModel(model, bitModel);
            mv.DataContext = mvm;
            mv.Show();
        }
    }
}
