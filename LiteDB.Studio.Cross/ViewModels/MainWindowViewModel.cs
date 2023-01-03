using CommunityToolkit.Mvvm.ComponentModel;

namespace LiteDB.Studio.Cross.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private ConnectionString _connectionString;
        //private const long MB = 1024 * 1024;
        private long MB = 1024 * 1024;
        
        [ObservableProperty]
        private bool _isLoadDatabaseNeeded = true;
        [ObservableProperty]
        private DbConnectionOptionsViewModel _connectionOpts;

        public MainWindowViewModel()
        {
            _connectionString = new ConnectionString();
            SetConnectionVM(_connectionString);
        }

        private DbConnectionOptionsViewModel SetConnectionVM(ConnectionString cs)
        {
            var result = new DbConnectionOptionsViewModel()
            {
                IsDirect = cs.Connection == ConnectionType.Direct,
                IsShared = cs.Connection == ConnectionType.Shared,
                DbPath = cs.Filename,
                Password = cs.Password,
                InitSize = (cs.InitialSize/MB).ToString(),
                IsReadOnly = cs.ReadOnly,
                IsUpgrade = cs.Upgrade
            };
            if (cs.Collation != null)
            {
                result.Culture = _connectionString.Collation.Culture.ToString();
                result.Sort = cs.Collation.SortOptions.ToString();
            }
            return result;
        }

        private void ConfigureConnectionString(DbConnectionOptionsViewModel vm)
        {
            _connectionString.Connection = vm.IsDirect ? ConnectionType.Direct : ConnectionType.Shared;
            _connectionString.Filename = vm.DbPath;
            _connectionString.ReadOnly = vm.IsReadOnly;
            _connectionString.Upgrade = vm.IsUpgrade;
            _connectionString.Password = vm.Password?.Trim().Length > 0 ? vm.Password?.Trim() : null;
            if (int.TryParse(vm.InitSize,out var initSize))
            {
                _connectionString.InitialSize = initSize * MB;
            }

            if (!string.IsNullOrEmpty(vm.Culture))
            {
                var collation = vm.Culture;
                if (!string.IsNullOrEmpty(vm.Sort))
                    collation += "/" + vm.Sort;
                _connectionString.Collation = new Collation(collation);
            }
        }
    }
}