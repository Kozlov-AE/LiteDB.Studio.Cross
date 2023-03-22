using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiteDB.Studio.Cross.Contracts.DTO;
using LiteDB.Studio.Cross.Models.EventArgs;
using LiteDB.Studio.Cross.Services;
using System.Collections.Generic;

namespace LiteDB.Studio.Cross.ViewModels;
public partial class DatabaseViewModel: ViewModelBase {
    private readonly ConnectionsManager _cManager;
    public string Id { get; }
    [ObservableProperty] private string _name;
    [ObservableProperty] private CollectionSetViewModel _sysCollections;
    [ObservableProperty] private CollectionSetViewModel _dbCollections;
    [ObservableProperty] private DataBaseWorkspaceViewModel _workspace;

    public DatabaseViewModel(ConnectionsManager cManager, DataBaseDto dto) {
        _cManager = cManager;
        Id = dto.Id;
        Name = dto.Name;
        SetSysCollections(dto.SysCollections);
        SetDbCollections(dto.DbCollections);
        Workspace = new DataBaseWorkspaceViewModel();
        Workspace.SendQueryEvent += OnWorkSpaceSendQuery;
    }

    private void OnWorkSpaceSendQuery(object? sender, DataBaseWorkspaceQueryEventArgs e) {
        var qResult = _cManager.SendQuery(Id, e.QueryText);
        if (qResult != null) {
            ((DataBaseWorkspaceViewModel)sender!).SetQueryResult(qResult, e.QueryVmName);
        }
    }

    private void CollectionVmOnAskedLoadItems(object? sender, int e) {
        var result = _cManager.GetItems(((CollectionViewModel)sender!)?.Name, e, Id);
        if (result != null) {
            Workspace.SelectedQueryVm.SetQueryResult(result);
        }
    }

    private CollectionViewModel CreateCollectionViewModel(string type, string name) {
        CollectionViewModel vm = null;
        switch (type) {
            case nameof(SystemCollectionViewModel): 
                vm = new SystemCollectionViewModel(name);
                break;
            case nameof(CollectionViewModel):
                default:
                vm = new CollectionViewModel(name);
                break;
        }
        vm.OnAskedLoadItemsEvent += CollectionVmOnAskedLoadItems;
        return vm;
    }

    public void SetSysCollections(IEnumerable<string> collections) {
        SysCollections = new CollectionSetViewModel { Name = "System collections" };
        foreach (var name in collections) {
            SysCollections.Collections.Add(CreateCollectionViewModel(nameof(SystemCollectionViewModel), name));
        }
    }
    
    public void SetDbCollections(IEnumerable<string> collections) {
        DbCollections = new CollectionSetViewModel { Name = "User collections" };
        foreach (var name in collections) {
            DbCollections.Collections.Add(CreateCollectionViewModel(nameof(CollectionViewModel), name));
        }
    }
    
    
    [RelayCommand]
    private void Disconnect() {
        _cManager.Disconnect(Id);
    }
}