using Avalonia.Controls.Shapes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiteDB.Studio.Cross.Interfaces;
using System.Collections.ObjectModel;
using System.Linq;
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
    }
}