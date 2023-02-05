using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LiteDB.Studio.Cross.ViewModels {
    /// <summary>
    /// ViewModel for the workspace
    /// </summary>
    public partial class DataBaseWorkspaceViewModel : ViewModelBase {
        [ObservableProperty] private ObservableCollection<QueryViewModel> _queries;
    }
    /// <summary>
    /// ViewModel for a query
    /// </summary>
    public partial class QueryViewModel : ViewModelBase {
        [ObservableProperty] private string _query;
        [ObservableProperty] private DbTableViewModel _tableVm;
        [ObservableProperty] private string _json;
    }
    /// <summary>
    /// ViewModel for a table presentation of request result
    /// </summary>
    public partial class DbTableViewModel : ViewModelBase {
        [ObservableProperty] private ObservableCollection<FieldViewModel> _fields;
        [ObservableProperty] private ObservableCollection<TableRowViewModel> _rows;
    }
    public partial class TableRowViewModel : ViewModelBase {
        [ObservableProperty] private Dictionary<string, string> _items;
    }
}

