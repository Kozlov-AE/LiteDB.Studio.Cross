using CommunityToolkit.Mvvm.ComponentModel;
using LiteDB.Studio.Cross.Interfaces;
using LiteDB.Studio.Cross.Services;
using System.Collections.ObjectModel;
using Tmds.DBus;

namespace LiteDB.Studio.Cross.ViewModels;
public partial class DataBaseExplorerViewModel: ViewModelBase {
    [ObservableProperty] private ObservableCollection<DatabaseViewModel> _databases;
    [ObservableProperty] private DatabaseViewModel _selectedDbVm;
    public DataBaseExplorerViewModel() {
        Databases = new();
    }
}
