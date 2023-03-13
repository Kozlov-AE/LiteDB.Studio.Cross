using CommunityToolkit.Mvvm.ComponentModel;
using LiteDB.Studio.Cross.Interfaces;
using LiteDB.Studio.Cross.Services;
using System.Collections.ObjectModel;
using System.Linq;
using Tmds.DBus;

namespace LiteDB.Studio.Cross.ViewModels;
public partial class DataBaseExplorerViewModel: ViewModelBase {
    private readonly ConnectionsManager _cManager;

    [ObservableProperty] private ObservableCollection<DatabaseViewModel> _databases;
    [ObservableProperty] private DatabaseViewModel _selectedDbVm;
    public DataBaseExplorerViewModel(ConnectionsManager cManager) {
        _cManager = cManager;
        _cManager.DatabaseDisconnected += CManagerOnDatabaseDisconnected;
        Databases = new();
    }

    private void CManagerOnDatabaseDisconnected(string id) {
        var db = Databases.FirstOrDefault(d => d.Id == id);
        if (db != null) Databases.Remove(db);
    }
}
