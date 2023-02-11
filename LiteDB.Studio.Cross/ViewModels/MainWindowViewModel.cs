using AvaloniaEdit.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiteDB.Studio.Cross.Contracts.DTO;
using LiteDB.Studio.Cross.Interfaces;
using LiteDB.Studio.Cross.Models;
using LiteDB.Studio.Cross.Models.EventArgs;
using LiteDB.Studio.Cross.Services;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;

namespace LiteDB.Studio.Cross.ViewModels {
    public partial class MainWindowViewModel : ViewModelBase {
        private readonly IOpenDbHistoryService _historyService;
        private readonly ConnectionsManager _connectionsManager;
        private readonly IMapper _mapper;
        private readonly ViewModelsFactory _vmFactory;

        [ObservableProperty] private bool _isLoadDatabaseNeeded = true;

        [ObservableProperty] private DataBaseExplorerViewModel _dbExplorerVm;
        [ObservableProperty] private DataBaseWorkspaceViewModel _dbWorkspaceVm;
        [ObservableProperty] private ConnectionParametersViewModel _connectionOpts;
        
        public event Action<DbQuerryResultModel> QueryFinished;

        public MainWindowViewModel(
                    IOpenDbHistoryService historyService, 
                    ConnectionsManager connectionsManager,
                    IMapper mapper, ViewModelsFactory vmFactory) {
            _connectionsManager = connectionsManager;
            _mapper = mapper;
            _vmFactory = vmFactory;
            _historyService = historyService;
            
            var dbE = _vmFactory.GetViewModel(typeof(DataBaseExplorerViewModel));
            if (dbE is DataBaseExplorerViewModel dbe) DbExplorerVm = dbe;
            var cP = _vmFactory.GetViewModel(typeof(ConnectionParametersViewModel));
            if (cP is ConnectionParametersViewModel cp) ConnectionOpts = cp;

        }
        
        [RelayCommand] private void Connect(ConnectionParametersViewModel vm) {
            if (DbExplorerVm.Databases.Any(d => d.Id == vm.DbPath)) return;
            var connection = _connectionsManager.Connect(_mapper.Map<ConnectionParametersDto>(vm));
            if (connection != null) {
                var dbVm = _vmFactory.GetDatabaseViewModel(connection);
                DbExplorerVm.Databases.Add(dbVm);
                IsLoadDatabaseNeeded = false;
            }
        }
        
        [RelayCommand]
        private void AskConnection() {
            IsLoadDatabaseNeeded = true;
        }
    }
}