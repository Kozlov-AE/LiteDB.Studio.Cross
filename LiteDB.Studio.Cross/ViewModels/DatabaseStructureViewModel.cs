using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using LiteDB.Studio.Cross.Models;
using System.Collections.ObjectModel;

namespace LiteDB.Studio.Cross.ViewModels {
    public partial class DatabaseStructureViewModel : ViewModelBase {
        [ObservableProperty] private string? _dbName;
        [ObservableProperty] private DatabaseStructureViewModel _sysDirectory;
        [ObservableProperty] private ObservableCollection<DbCollectionViewModel> _collections = new ObservableCollection<DbCollectionViewModel>();
    }
    
    public partial class DbCollectionViewModel : ViewModelBase {
        [ObservableProperty] private string? _collectionName;
        [ObservableProperty] private ObservableCollection<PropertyModel> _fields;
        [ObservableProperty] private ObservableCollection<dynamic> _items = new ObservableCollection<dynamic>();
    }
}