using CommunityToolkit.Mvvm.Input;
using LiteDB.Studio.Cross.Models;
using LiteDB.Studio.Cross.ViewModels;
using System.Collections.ObjectModel;

namespace LiteDB.Studio.Cross.Interfaces
{
    public interface IConnectionsExplorerViewModel {
        ObservableCollection<ConnectionModel> Connections { get; set; }
        ConnectionModel SelectedConnection { get; set; }
    }
}