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
    public partial class MainWindowViewModel : ViewModelBase, IMainWindowViewModel {
        private readonly IOpenDbHistoryService _historyService;
        private readonly DataBaseConnectionsManagerService _connectionsManagerService;
        private readonly IMapper _mapper;
        private readonly ViewModelsFactory _vmFactory;

        [ObservableProperty] private bool _isLoadDatabaseNeeded = true;

        [ObservableProperty] private DataBaseExplorerViewModel _dbExplorerVm;
        [ObservableProperty] private DataBaseWorkspaceViewModel _dbWorkspaceVm;
        [ObservableProperty] private ConnectionParametersViewModel _connectionOpts;
        
        public event Action<DbQuerryResultModel> QueryFinished;

        public MainWindowViewModel(
                    IOpenDbHistoryService historyService, 
                    DataBaseConnectionsManagerService connectionsManagerService,
                    IMapper mapper, ViewModelsFactory vmFactory) {
            _connectionsManagerService = connectionsManagerService;
            _mapper = mapper;
            _vmFactory = vmFactory;
            _historyService = historyService;
            
            var dbE = _vmFactory.GetViewModel(typeof(DataBaseExplorerViewModel));
            if (dbE is DataBaseExplorerViewModel dbe) DbExplorerVm = dbe;
            var dbW = _vmFactory.GetViewModel(typeof(DataBaseWorkspaceViewModel));
            if (dbW is DataBaseWorkspaceViewModel dbw) DbWorkspaceVm = dbw;
            var cP = _vmFactory.GetViewModel(typeof(ConnectionParametersViewModel));
            if (cP is ConnectionParametersViewModel cp) ConnectionOpts = cp;

        }

        [RelayCommand] private void Connect(ConnectionParametersViewModel vm) {
            if (DbExplorerVm.Databases.Any(d => d.Id == vm.DbPath)) return;
            var connection = _connectionsManagerService.Connect(_mapper.Map<ConnectionParametersDto>(vm));
            var dbVm = _mapper.Map<DatabaseViewModel>(connection);
            DbExplorerVm.Databases.Add(dbVm);
            IsLoadDatabaseNeeded = false;
        }
        [RelayCommand]
        private void AskConnection() {
            IsLoadDatabaseNeeded = true;
        }

        [RelayCommand]
        private void SendQuery(string text) {
            if (ConnectionsExplorer.SelectedConnection == null) return;
            var res = ConnectionsExplorer.SelectedConnection.SendQuery(text);
            var options = new JsonSerializerOptions { WriteIndented = true };
            QueryResultString = System.Text.Json.JsonSerializer.Serialize(res.Items, options);
            QueryFinished?.Invoke(res);
        }
    }
}