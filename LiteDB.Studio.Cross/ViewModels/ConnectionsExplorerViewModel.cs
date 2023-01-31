using AvaloniaEdit.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiteDB.Studio.Cross.Interfaces;
using LiteDB.Studio.Cross.Models;
using System.Collections.ObjectModel;

namespace LiteDB.Studio.Cross.ViewModels {
    public partial class ConnectionsExplorerViewModel : ViewModelBase {
        [ObservableProperty] private ObservableCollection<ConnectionExplorerItemViewModel> _connections;
        [ObservableProperty] private ConnectionExplorerItemViewModel _selectedConnection;

        public ConnectionsExplorerViewModel() {
            Connections = new ObservableCollection<ConnectionExplorerItemViewModel>();
        }
        
        [RelayCommand]
        private void Disconnect(ConnectionExplorerItemViewModel conn) {
            if (conn.Disconnect()) Connections.Remove(conn);
        }
    }
}
