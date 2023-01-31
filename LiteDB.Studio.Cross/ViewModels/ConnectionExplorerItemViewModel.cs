using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiteDB.Studio.Cross.Models;
using System.Collections.ObjectModel;

namespace LiteDB.Studio.Cross.ViewModels {
    public partial class ConnectionExplorerItemViewModel : ViewModelBase {
        private ConnectionModel _connection;

        [ObservableProperty] private string _dbName;
        [ObservableProperty] private ObservableCollection<DbCollectionViewModel> _systemCollections;
        [ObservableProperty] private ObservableCollection<DbCollectionViewModel> _collections;

        public bool Disconnect() {
            return _connection.Disconnect();
        }
    }
    
    
}