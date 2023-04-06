using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiteDB.Studio.Cross.Contracts.DTO;
using LiteDB.Studio.Cross.Models.EventArgs;
using LiteDB.Studio.Cross.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace LiteDB.Studio.Cross.ViewModels {
    /// <summary>
    /// ViewModel for the workspace
    /// </summary>
    public partial class DataBaseWorkspaceViewModel : ViewModelBase {
        private readonly ConnectionsManager _connectionsManager;
        private readonly ViewModelsFactory _vmFactory;
        private string _connectionId;
        [ObservableProperty] private ObservableCollection<QueryViewModel> _queries;
        [ObservableProperty] private QueryViewModel _selectedQueryVm;

        public event EventHandler<DataBaseWorkspaceQueryEventArgs> SendQueryEvent; 

        public DataBaseWorkspaceViewModel() {
            Queries = new ObservableCollection<QueryViewModel>();
            AddQueryModel();
        }
        public void AddQueryModel() {
            var vm = new QueryViewModel();
            vm.Name = (Queries.Count + 1).ToString();
            vm.SendQueryEvent += SendQueryEventHandler;
            Queries.Add(vm);
            SelectedQueryVm = vm;
        }
        
        public void SetQueryResult(QueryResultDto dto, string queryViewName) {
            var qv = Queries.FirstOrDefault(q => q.Name == queryViewName);
            if (qv == null) return;
            qv.SetQueryResult(dto);
        }

        private void SendQueryEventHandler(object? sender, string text) {
            OnSendQueryEvent(new DataBaseWorkspaceQueryEventArgs(((QueryViewModel)sender!).Name, text));
        }

        protected virtual void OnSendQueryEvent(DataBaseWorkspaceQueryEventArgs e) {
            SendQueryEvent?.Invoke(this, e);
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

        public event EventHandler<string> SendQueryEvent;

        public QueryViewModel() {
            TableVm = new();
        }
        [RelayCommand]
        private async Task SendQuery(string text) {
            OnSendQueryEvent(text);
        }

        public void SetQueryResult(QueryResultDto dto) {
            Query = dto.QueryText;
            TableVm.SetTable(dto);
            var sOpts = new JsonSerializerOptions { WriteIndented = true };
            var json = System.Text.Json.JsonSerializer.Serialize(dto.Items, sOpts);
            Json = json;
        }
        protected virtual void OnSendQueryEvent(string e) {
            SendQueryEvent?.Invoke(this, e);
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
            Fields.Clear();
            Rows.Clear();
            foreach (var field in dto.Fields) {
                DbCollectionFieldViewModel fVm = new DbCollectionFieldViewModel() { Name = field };
                Fields.Add(fVm);
            }
            foreach (var item in dto.Items){
                if (item == null) continue;
                var row = new TableRowViewModel();
                foreach (var cell in item){
                    row.Items.Add(cell.Key, cell.Value);
                }
                Rows.Add(row);
            }
            TableUpdated?.Invoke();
        }
    }
    public partial class TableRowViewModel : ViewModelBase {
        [ObservableProperty] private Dictionary<string, dynamic> _items;

        public TableRowViewModel() {
            Items = new();
        }
    }
}

