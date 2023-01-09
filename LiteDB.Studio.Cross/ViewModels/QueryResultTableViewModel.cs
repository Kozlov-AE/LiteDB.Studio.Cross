using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace LiteDB.Studio.Cross.ViewModels {
    public partial class QueryResultTableViewModel : ViewModelBase {
        [ObservableProperty] private ObservableCollection<string> _headers;
    }
}