using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace LiteDB.Studio.Cross.ViewModels;
public partial class DataBaseExplorerViewModel: ViewModelBase {
    [ObservableProperty] private ObservableCollection<DatabaseViewModel> _databases;

    public DataBaseExplorerViewModel() {
        Databases = new();
    }

    private void AddDatabase(DatabaseViewModel vm) {
        Databases.Add(vm);
    }
}
