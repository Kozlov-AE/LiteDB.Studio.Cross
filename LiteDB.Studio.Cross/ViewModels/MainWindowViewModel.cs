using Avalonia.Controls.Shapes;
using AvaloniaEdit.Document;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiteDB.Studio.Cross.Interfaces;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using Path = System.IO.Path;

namespace LiteDB.Studio.Cross.ViewModels {
    public partial class MainWindowViewModel : ViewModelBase, IMainWindowViewModel {
        private ConnectionString _connectionString;
        private LiteDatabase? _db = null;

        private const long MB = 1024 * 1024;

        [ObservableProperty] private bool _isLoadDatabaseNeeded = true;
        [ObservableProperty] private DbConnectionOptionsViewModel _connectionOpts;
        [ObservableProperty] private DatabaseStructureViewModel _structureViewModel;
        [ObservableProperty] private bool _isDbConnected;
        [ObservableProperty] private string _queryString;
        [ObservableProperty] private string _queryResultString;
        public MainWindowViewModel() {
            _connectionString = new ConnectionString();
            ConnectionOpts = SetConnectionVm(_connectionString);
            StructureViewModel = new DatabaseStructureViewModel();
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
        private void Disconnect() {
            _db?.Dispose();
            IsDbConnected = false; 
        }
        [RelayCommand]
        private void AskConnection() {
            IsLoadDatabaseNeeded = true;
        }
        [RelayCommand]
        private void ConnectToDatabase() {
            _connectionString = ConfigureConnectionString(_connectionOpts);
            Disconnect();
            _db = new LiteDatabase(_connectionString);
            StructureViewModel = new DatabaseStructureViewModel();
            StructureViewModel.DbName = Path.GetFileName(_connectionString.Filename);
            
            StructureViewModel.SysDirectory = new DatabaseStructureViewModel();
            StructureViewModel.SysDirectory.DbName = "System";
            var sc = _db.GetCollection("$cols")
                .Query()
                .Where("type = 'system'")
                .OrderBy("name")
                .ToDocuments();
            StructureViewModel.SysDirectory.Collections = new ObservableCollection<DbCollectionViewModel>();
            foreach (var doc in sc) {
                var collection = new DbCollectionViewModel();
                collection.CollectionName = doc["name"].AsString;
                collection.Fields = new ObservableCollection<string>();
                foreach (var key in doc.Keys) {
                    collection.Fields.Add(key);
                }
                StructureViewModel.SysDirectory.Collections.Add(collection);
            }

            StructureViewModel.Collections = new ObservableCollection<DbCollectionViewModel>();
            var colls = _db.GetCollectionNames().OrderBy(x => x);
            foreach (var name in colls) {
                var coll = new DbCollectionViewModel();
                coll.CollectionName = name;
                coll.Fields = new ObservableCollection<string>();
                StructureViewModel.Collections.Add(coll);
            }

            IsDbConnected = true;
            IsLoadDatabaseNeeded = false;
        }
        [RelayCommand]
        private void SendQuery(string text) {
            QueryResultString = String.Empty; 
            var query = text.Replace(Environment.NewLine, " ");
            if (string.IsNullOrWhiteSpace(query) || _db == null) return;
            var doc = new BsonDocument();
            var sql = new StringReader(query);
            var result = new List<BsonValue>();
            using (var reader = _db.Execute(sql, doc)) {
                while (reader.Read()) {
                    result.Add(reader.Current);
                }
            }
            
            if (!result.Any()) return;
            var sb = new StringBuilder();
            using (var writer = new StringWriter(sb)) {
            var json = new JsonWriter(writer) { Pretty = true, Indent = 2 };
                foreach (var d in result) {
                    json.Serialize(d);
                    sb.AppendLine();
                }
            }
            QueryResultString = sb.ToString();
        }
    }
}