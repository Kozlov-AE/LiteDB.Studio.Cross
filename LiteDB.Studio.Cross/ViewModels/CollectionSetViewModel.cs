using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace LiteDB.Studio.Cross.ViewModels;
public partial class CollectionSetViewModel: ViewModelBase {
    [ObservableProperty] private string _name;
    [ObservableProperty] private ObservableCollection<CollectionViewModel> _collections;

    public CollectionSetViewModel() {
        Collections = new();
    }
}