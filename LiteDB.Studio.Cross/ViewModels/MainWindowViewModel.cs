using Avalonia.Controls.Shapes;
using AvaloniaEdit.Document;
using AvaloniaEdit.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiteDB.Studio.Cross.Interfaces;
using LiteDB.Studio.Cross.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using Path = System.IO.Path;
using PropertyModel = LiteDB.Studio.Cross.Models.PropertyModel;

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
                collection.Fields = new ObservableCollection<PropertyModel>();
                foreach (var d in doc) {
                    if (collection.Fields.Any(f => f.Name == d.Key)) collection.Fields.Add(GetDbValueType(d));
                }
                StructureViewModel.SysDirectory.Collections.Add(collection);
            }

            StructureViewModel.Collections = new ObservableCollection<DbCollectionViewModel>();
            var colls = _db.GetCollectionNames().OrderBy(x => x);
            foreach (var name in colls) {
                var coll = new DbCollectionViewModel();
                coll.CollectionName = name;
                coll.Fields = new ObservableCollection<PropertyModel>();
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
            var fields = new HashSet<PropertyModel>(30);
            var sb = new StringBuilder();
            Dictionary<string, dynamic> data = new Dictionary<string, dynamic>();
            Type type = null;
            using (var reader = _db.Execute(sql, doc)) {
                var dc = StructureViewModel.Collections.FirstOrDefault(n =>
                    n.CollectionName == reader.Collection);
                dc.Items.Clear();

                while (reader.Read()) {
                    var bson = reader.Current;
                    var docs = bson.AsDocument;
                    var isAdded = false;
                    data.Clear();
                    try {
                        foreach (var value in docs) {
                            dynamic dataVal = null;
                            var val = value.Value;
                            if (dc.Fields.All(f => f.Name != value.Key)) {
                                dc.Fields.Add(GetDbValueType(value));
                                isAdded = true;
                            }

                            if (val.IsDateTime) dataVal = val.AsDateTime;
                            else if (val.IsBoolean) dataVal = val.AsBoolean;
                            else if (val.IsDecimal) dataVal = val.AsDecimal;
                            else if (val.IsDouble) dataVal = val.AsDouble;
                            else if (val.IsInt32) dataVal = val.AsInt32;
                            else if (val.IsInt64) dataVal = val.AsInt64;
                            else if (val.IsString) dataVal = val.AsString;
                            else if (val.IsBinary) dataVal = val.AsBinary;
                            else dataVal = val.ToString();

                            data.Add(value.Key, dataVal);
                        }

                        if (isAdded)
                            type = DbCollectionClassGenerator.GenerateCollectionClass(fields, reader.Collection);

                        var o = DbCollectionClassGenerator.GetObject(type, data);
                        dc.Items.Add(o);
                    }
                    catch (Exception ex) {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        private PropertyModel GetDbValueType(KeyValuePair<string, BsonValue> pair) {
            Type type;
            var value = pair.Value;
            if (value.IsString) type = typeof(string);
            else if (value.IsBoolean) type = typeof(bool);
            else if (value.IsBinary) type = typeof(byte[]);
            else if (value.IsDecimal) type = typeof(decimal);
            else if (value.IsDouble) type = typeof(double);
            else if (value.IsInt32) type = typeof(int);
            else if (value.IsInt64) type = typeof(long);
            else if (value.IsDateTime) type = typeof(DateTime);
            else type = typeof(string);
            return new PropertyModel(pair.Key, type);
        }
    }
}