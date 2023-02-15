using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiteDB.Studio.Cross.Contracts.DTO;
using LiteDB.Studio.Cross.Interfaces;
using LiteDB.Studio.Cross.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LiteDB.Studio.Cross.ViewModels {
    /// <summary>
    /// ViewModel for the workspace
    /// </summary>
    public partial class DataBaseWorkspaceViewModel : ViewModelBase, IDataBaseWorkspaceViewModel {
        private readonly ConnectionsManager _connectionsManager;
        private readonly ViewModelsFactory _vmFactory;
        private string _connectionId;
        [ObservableProperty] private ObservableCollection<QueryViewModel> _queries;

        public DataBaseWorkspaceViewModel(ConnectionsManager connectionsManager, ViewModelsFactory vmFactory) {
            _connectionsManager = connectionsManager;
            _vmFactory = vmFactory;
            Queries = new ObservableCollection<QueryViewModel>();
        }
        public void SetConnectionId(string id) {
            _connectionId = id;
        }

        public void AddQueryModel() {
            var newName = (Queries.Count + 1).ToString();
            var vm = _vmFactory.GetQueryVm(newName, _connectionId);
            Queries.Add(vm);
        }

    }
    /// <summary>
    /// ViewModel for a query
    /// </summary>
    public partial class QueryViewModel : ViewModelBase {
        private string _connectionId;
        
        [ObservableProperty] private string _name;
        [ObservableProperty] private string _query;
        [ObservableProperty] private DbTableViewModel _tableVm;
        [ObservableProperty] private string _json;
        
        private readonly ConnectionsManager _connectionsManager;

        public QueryViewModel(ConnectionsManager connectionsManager) {
            _connectionsManager = connectionsManager;
            TableVm = new();
        }
        [RelayCommand]
        private async Task SendQuery(string text) {
            var res = await Task.Run(() => _connectionsManager.SendQuery(_connectionId, text));
            if (res != null){
                TableVm.SetTable(res);
                var sOpts = new JsonSerializerOptions { WriteIndented = true };
                var json = System.Text.Json.JsonSerializer.Serialize(res.Items, sOpts);
                Json = json;
            }
        }
        public void SetConnectionId(string id) {
            _connectionId = id;
        }
    }
    /// <summary>
    /// ViewModel for a table presentation of request result
    /// </summary>
    public partial class DbTableViewModel : ViewModelBase {
        [ObservableProperty] private ObservableCollection<DbCollectionFieldViewModel> _fields;
        [ObservableProperty] private ObservableCollection<TableRowViewModel> _rows;

        public event Action TableUpdated;

        public DbTableViewModel() {
            Fields = new ();
            Rows = new();
        }

        public void SetTable(QueryResultDto dto) {
            foreach (var field in dto.Fields) {
                DbCollectionFieldViewModel fVm = new DbCollectionFieldViewModel() { Name = field };
                Fields.Add(fVm);
            }
            foreach (var item in dto.Items){
                if (item == null) continue;
                var row = new TableRowViewModel();
                foreach (var cell in item.Cells){
                    row.Items.Add(cell.Name, cell.Value.ToString());
                }
                Rows.Add(row);
            }
            TableUpdated?.Invoke();
        }
    }
    public partial class TableRowViewModel : ViewModelBase {
        [ObservableProperty] private Dictionary<string, string> _items;

        public TableRowViewModel() {
            Items = new();
        }
    }
}

