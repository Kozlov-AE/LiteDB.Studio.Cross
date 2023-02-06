﻿using AvaloniaEdit.Utils;
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
        
        [ObservableProperty] private DataBaseExplorerViewModel _dbExplorerVm;
        [ObservableProperty] private DataBaseWorkspaceViewModel _dbWorkspaceVm;

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
        }


        [RelayCommand] private void Connect(ConnectionParametersViewModel vm) {
            var connection = _connectionsManagerService.Connect(_mapper.Map<ConnectionParametersDto>(vm));
            var dbVm = _mapper.Map<DatabaseViewModel>(connection);
            DbExplorerVm.Databases.Add(dbVm);
        }
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        private readonly DatabaseService_OLD _dbServiceOld; 
        private ConnectionString _connectionString;
        private List<RightSideViewModel> _rightSideViewModels;

        private const long MB = 1024 * 1024;

        [ObservableProperty] private bool _isLoadDatabaseNeeded = true;
        [ObservableProperty] private bool _isDbConnected;
        [ObservableProperty] private string _queryString;
        [ObservableProperty] private string _queryResultString;
        [ObservableProperty] private string _selectedHistoryItem;
        [ObservableProperty] private ObservableCollection<string> _openDbHistory;
        
        [ObservableProperty] private ConnectionParametersViewModel _connectionOpts;
        [ObservableProperty] private DatabaseStructureViewModel _structureViewModel;
        [ObservableProperty] private ConnectionsExplorerViewModel _connectionsExplorer;
        [ObservableProperty] private RightSideViewModel _rightSideViewModel;

        public event Action<DbQuerryResultModel> QueryFinished;

        public MainWindowViewModel(IOpenDbHistoryService historyService, IMapper mapper, ViewModelsFactory vmFactory) {
            _dbServiceOld = new DatabaseService_OLD();
            _connectionString = new ConnectionString();
            _historyService = historyService;
            _mapper = mapper;
            _vmFactory = vmFactory;

            ConnectionsExplorer = new ConnectionsExplorerViewModel();
            ConnectionOpts = SetConnectionVm(_connectionString);
            StructureViewModel = new DatabaseStructureViewModel();
            OpenDbHistory = new ObservableCollection<string>();
            OpenDbHistory.AddRange(historyService.GetHistory());
            
            this.PropertyChanged += OnPropertyChanged;
            ConnectionOpts.PropertyChanged += ConnectionOptsOnPropertyChanged;
            historyService.OpenDbHistoryChanged += HistoryServiceOnOpenDbHistoryChanged;
        }

        private void ConnectionOptsOnPropertyChanged(object? sender, PropertyChangedEventArgs e) {
            switch (e.PropertyName) {
                case "DbPath":
                    SelectedHistoryItem = string.Empty;
                    break;
            }
        }

        private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e) {
            switch (e.PropertyName) {
                case "SelectedHistoryItem":
                    if (!string.IsNullOrEmpty(SelectedHistoryItem))
                        ConnectionOpts.DbPath = SelectedHistoryItem;
                    break;
            }
        }

        private void HistoryServiceOnOpenDbHistoryChanged(object? sender, OpenDbHistoryEventArgs e) {
            switch (e.EventType) {
                case OpenDbHistoryEventTypes.PathAdded:
                    OpenDbHistory.Add(e.ChangedPath);
                    break;
                case OpenDbHistoryEventTypes.PathRemoved:
                    var p = OpenDbHistory.FirstOrDefault(p => p == e.ChangedPath);
                    if (!string.IsNullOrWhiteSpace(p)) OpenDbHistory.Remove(p);
                    break;
            }
        }

        private ConnectionParametersViewModel SetConnectionVm(ConnectionString cs) {
            var result = new ConnectionParametersViewModel() {
                IsDirect = cs.Connection == ConnectionType.Direct,
                IsShared = cs.Connection == ConnectionType.Shared,
                DbPath = cs.Filename,
                Password = cs.Password,
                InitSize = (cs.InitialSize / MB).ToString(),
                IsReadOnly = cs.ReadOnly,
                IsUpgrade = cs.Upgrade
            };
            if (cs.Collation != null) {
                result.Culture = _connectionString.Collation.Culture.ToString();
                result.Sort = cs.Collation.SortOptions.ToString();
            }

            return result;
        }
        private ConnectionString ConfigureConnectionString(ConnectionParametersViewModel vm) {
            var cs = new ConnectionString();
            cs.Connection = vm.IsDirect ? ConnectionType.Direct : ConnectionType.Shared;
            cs.Filename = vm.DbPath;
            cs.ReadOnly = vm.IsReadOnly;
            cs.Upgrade = vm.IsUpgrade;
            cs.Password = vm.Password?.Trim().Length > 0 ? vm.Password?.Trim() : null;
            if (int.TryParse(vm.InitSize, out var initSize)) {
                cs.InitialSize = initSize * MB;
            }

            if (!string.IsNullOrEmpty(vm.Culture)) {
                var collation = vm.Culture;
                if (!string.IsNullOrEmpty(vm.Sort)) {
                    collation += "/" + vm.Sort;
                }

                cs.Collation = new Collation(collation);
            }

            return cs;
        }

        [RelayCommand]
        private void AskConnection() {
            IsLoadDatabaseNeeded = true;
        }
        [RelayCommand]
        private void ConnectToDatabase() {
            if (ConnectionsExplorer.Connections.Any(c => c.FileName == ConnectionOpts.DbPath) || string.IsNullOrWhiteSpace(ConnectionOpts.DbPath)) return;
            var connect = new ConnectionModel();
            _connectionString = ConfigureConnectionString(ConnectionOpts);
            if (!connect.Connect(_connectionString)) return;
            ConnectionsExplorer.Connections.Add(connect);
            _historyService.AddToStory(ConnectionOpts.DbPath);
            IsLoadDatabaseNeeded = false;
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