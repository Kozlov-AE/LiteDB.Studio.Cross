using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace LiteDB.Studio.Cross.ViewModels {
    public partial class DatabaseStructureViewModel : ViewModelBase {
        [ObservableProperty] private string? _dbName;
        [ObservableProperty] private DatabaseStructureViewModel _sysDirectory;
        [ObservableProperty] private ObservableCollection<DbCollectionViewModel> _collections;
    }
    
    public partial class DbCollectionViewModel : ViewModelBase {
        [ObservableProperty] private string? _collectionName;
        [ObservableProperty] private ObservableCollection<string> _fields;
    }
}