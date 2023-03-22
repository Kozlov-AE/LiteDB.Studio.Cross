using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiteDB.Studio.Cross.Models;
using LiteDB.Studio.Cross.Services;
using System;
using System.Linq;

namespace LiteDB.Studio.Cross.ViewModels {
    public partial class MainWindowViewModel : ViewModelBase {
        private readonly OpenDbHistoryService _historyService;
        private readonly ConnectionsManager _connectionsManager;
        private readonly ViewModelsFactory _vmFactory;

        [ObservableProperty] private bool _isLoadDatabaseNeeded = true;

        [ObservableProperty] private DataBaseExplorerViewModel _dbExplorerVm;
        [ObservableProperty] private DataBaseWorkspaceViewModel _dbWorkspaceVm;
        [ObservableProperty] private ConnectionParametersViewModel _connectionOpts;
        
        public MainWindowViewModel(
                    OpenDbHistoryService historyService, 
                    ConnectionsManager connectionsManager,
                    ViewModelsFactory vmFactory) {
            _connectionsManager = connectionsManager;
            _vmFactory = vmFactory;
            _historyService = historyService;

            ConnectionOpts = _vmFactory.GetConnectionParametersViewModel();
            var dbE = _vmFactory.GetViewModel(typeof(DataBaseExplorerViewModel));
            if (dbE is DataBaseExplorerViewModel dbe) DbExplorerVm = dbe;
        }
        
        [RelayCommand] private void Connect(ConnectionParametersViewModel vm) {
            if (DbExplorerVm.Databases.Any(d => d.Id == vm.DbPath)) return;
            var connection = _connectionsManager.Connect(vm.Map());
            if (connection != null) {
                var dbVm = new DatabaseViewModel(_connectionsManager, connection);
                DbExplorerVm.Databases.Add(dbVm);
                IsLoadDatabaseNeeded = false;
                _historyService.AddToStory(vm.DbPath!);
                ConnectionOpts = null;
            }
        }
        
        [RelayCommand]
        private void AskConnection() {
            if (ConnectionOpts == null) ConnectionOpts = _vmFactory.GetConnectionParametersViewModel();
            IsLoadDatabaseNeeded = true;
        }
    }
}