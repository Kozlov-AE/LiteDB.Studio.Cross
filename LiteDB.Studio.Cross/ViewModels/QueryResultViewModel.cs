using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace LiteDB.Studio.Cross.ViewModels {
    public partial class QueryResultViewModel : ViewModelBase {
        [ObservableProperty] private ObservableCollection<string> _fields;
    }
}