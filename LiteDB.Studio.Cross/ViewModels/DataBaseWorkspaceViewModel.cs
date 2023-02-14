using CommunityToolkit.Mvvm.ComponentModel;
using LiteDB.Studio.Cross.Interfaces;
using LiteDB.Studio.Cross.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LiteDB.Studio.Cross.ViewModels {
    /// <summary>
    /// ViewModel for the workspace
    /// </summary>
    public partial class DataBaseWorkspaceViewModel : ViewModelBase, IDataBaseWorkspaceViewModel {
        private readonly ConnectionsManager _connectionsManager;
        private string _connectionId;
        [ObservableProperty] private ObservableCollection<QueryViewModel> _queries;

        public DataBaseWorkspaceViewModel(ConnectionsManager connectionsManager) {
            _connectionsManager = connectionsManager;
            Queries = new ObservableCollection<QueryViewModel>();
            Queries.Add(new QueryViewModel());
        }

        public void SetConnectionId(string id) {
            _connectionId = id;
        }
    }
    /// <summary>
    /// ViewModel for a query
    /// </summary>
    public partial class QueryViewModel : ViewModelBase {
        [ObservableProperty] private string _name;
        [ObservableProperty] private string _query;
        [ObservableProperty] private DbTableViewModel _tableVm;
        [ObservableProperty] private string _json;

        public QueryViewModel() {
            TableVm = new();
        }
    }
    /// <summary>
    /// ViewModel for a table presentation of request result
    /// </summary>
    public partial class DbTableViewModel : ViewModelBase {
        [ObservableProperty] private ObservableCollection<DbCollectionFieldViewModel> _fields;
        [ObservableProperty] private ObservableCollection<TableRowViewModel> _rows;

        public DbTableViewModel() {
            Fields = new ();
            Rows = new();
        }
    }
    public partial class TableRowViewModel : ViewModelBase {
        [ObservableProperty] private Dictionary<string, string> _items;

        public TableRowViewModel() {
            Items = new();
        }
    }
}

