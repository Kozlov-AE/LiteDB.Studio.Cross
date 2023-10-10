using CommunityToolkit.Mvvm.ComponentModel;
using LiteDB.Studio.Cross.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace LiteDB.Studio.Cross.ViewModels;
public partial class DataBaseExplorerViewModel: ViewModelBase {
    private readonly ConnectionsManager _cManager;

    [ObservableProperty] private ObservableCollection<DatabaseViewModel> _databases;
    [ObservableProperty] private DatabaseViewModel _selectedDbVm;
    public DataBaseExplorerViewModel(ConnectionsManager cManager) {
        _cManager = cManager;
        _cManager.DatabaseDisconnected += OnCManagerOnDatabaseDisconnected;
        cManager.QueryResultReceived += OnCManagerQueryResultReceived; 
        Databases = new();
    }

    private void OnCManagerQueryResultReceived(ConnectionsManager.QueryResultEventArgs args) {
        
    }

    private void OnCManagerOnDatabaseDisconnected(string id) {
        var db = Databases.FirstOrDefault(d => d.Id == id);
        if (db != null) Databases.Remove(db);
    }
}
