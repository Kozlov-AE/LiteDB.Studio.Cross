using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiteDB.Studio.Cross.Interfaces;
using LiteDB.Studio.Cross.Services;
using System.Collections.ObjectModel;

namespace LiteDB.Studio.Cross.ViewModels;
public partial class DatabaseViewModel: ViewModelBase {
    private readonly ConnectionsManager _cManager;
    public string Id { get; set; }
    [ObservableProperty] private string _name;
    [ObservableProperty] private CollectionSetViewModel _sysCollections;
    [ObservableProperty] private CollectionSetViewModel _dbCollections;
    [ObservableProperty] private IDataBaseWorkspaceViewModel _workspace;

    public DatabaseViewModel(ConnectionsManager cManager) {
        _cManager = cManager;
    }
    [RelayCommand]
    private void Disconnect() {
        _cManager.Disconnect(Id);
    }
}