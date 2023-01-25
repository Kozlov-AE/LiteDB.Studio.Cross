using Avalonia.Controls.Shapes;
using AvaloniaEdit.Document;
using AvaloniaEdit.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiteDB.Studio.Cross.Interfaces;
using LiteDB.Studio.Cross.Models;
using LiteDB.Studio.Cross.Services;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Path = System.IO.Path;
using PropertyModel = LiteDB.Studio.Cross.Models.PropertyModel;

namespace LiteDB.Studio.Cross.ViewModels {
    public partial class MainWindowViewModel : ViewModelBase, IMainWindowViewModel {
        private readonly DatabaseService _dbService; 
        
        private ConnectionString _connectionString;

        private const long MB = 1024 * 1024;

        [ObservableProperty] private bool _isLoadDatabaseNeeded = true;
        [ObservableProperty] private DbConnectionOptionsViewModel _connectionOpts;
        [ObservableProperty] private DatabaseStructureViewModel _structureViewModel;
        [ObservableProperty] private bool _isDbConnected;
        [ObservableProperty] private string _queryString;
        [ObservableProperty] private string _queryResultString;
        [ObservableProperty] private ConnectionsExplorerViewModel _connectionsExplorer;
        [ObservableProperty] private ObservableCollection<string> _openDbHistory = new ObservableCollection<string>();

        public event Action<DbQuerryResultModel> QueryFinished;

        public MainWindowViewModel(IOpenDbHistory history) {
            _dbService = new DatabaseService();
            _connectionString = new ConnectionString();

            ConnectionsExplorer = new ConnectionsExplorerViewModel();
            ConnectionOpts = SetConnectionVm(_connectionString);
            StructureViewModel = new DatabaseStructureViewModel();
            OpenDbHistory.AddRange(history.GetHistory());
        }

        private DbConnectionOptionsViewModel SetConnectionVm(ConnectionString cs) {
            var result = new DbConnectionOptionsViewModel() {
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
        private ConnectionString ConfigureConnectionString(DbConnectionOptionsViewModel vm) {
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
            if (ConnectionsExplorer.Connections.Any(c => c.FileName == ConnectionOpts.DbPath)) return;
            var connect = new ConnectionModel();
            _connectionString = ConfigureConnectionString(ConnectionOpts);
            if (!connect.Connect(_connectionString)) return;
            ConnectionsExplorer.Connections.Add(connect);
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