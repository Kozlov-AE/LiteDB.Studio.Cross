using AvaloniaEdit.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiteDB.Studio.Cross.Interfaces;
using LiteDB.Studio.Cross.Models;
using System.Collections.ObjectModel;

namespace LiteDB.Studio.Cross.ViewModels {
    public partial class ConnectionsExplorerViewModel : ViewModelBase {
        [ObservableProperty] private ObservableCollection<ConnectionModel> _connections;
        [ObservableProperty] private ConnectionModel _selectedConnection;

        public ConnectionsExplorerViewModel() {
            Connections = new ObservableCollection<ConnectionModel>();
        }

        [RelayCommand]
        private void Disconnect(ConnectionModel conn) {
            if (conn.Disconnect()) Connections.Remove(conn);
        }
    }
}
