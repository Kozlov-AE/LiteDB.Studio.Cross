using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiteDB.Studio.Cross.Contracts.DTO;
using LiteDB.Studio.Cross.Models.EventArgs;
using LiteDB.Studio.Cross.Services;
using System.Collections.Generic;

namespace LiteDB.Studio.Cross.ViewModels;
public partial class DatabaseViewModel: ViewModelBase {
    private readonly ConnectionsManager _cManager;
    private readonly ViewModelsFactory _vmFactory;
    public string Id { get; }
    [ObservableProperty] private string _name;
    [ObservableProperty] private CollectionSetViewModel _sysCollections;
    [ObservableProperty] private CollectionSetViewModel _dbCollections;
    [ObservableProperty] private DataBaseWorkspaceViewModel _workspace;

    public DatabaseViewModel(ConnectionsManager cManager, DataBaseDto dto, ViewModelsFactory vmFactory) {
        _cManager = cManager;
        _vmFactory = vmFactory;
        Id = dto.Id;
        Name = dto.Name;
        Workspace = vmFactory.GetDataBaseWorkspaceViewModel();
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
                var f = _cManager.GetItems(vm.Name, 100, Id);
                foreach (var field in f.Fields) {
                    vm.Fields.Add(new DbCollectionFieldViewModel() {
                        FType = "string", Name = field
                    });
                }
                break;
        }
        vm.OnAskedLoadItemsEvent += CollectionVmOnAskedLoadItems;
        return vm;
    }

    [RelayCommand]
    private void Disconnect() {
        _cManager.Disconnect(Id);
    }
}