using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;


namespace LiteDB.Studio.Cross.ViewModels;
public partial class CollectionViewModel: ViewModelBase {
    [ObservableProperty] private string _name;
    [ObservableProperty] private ObservableCollection<FieldViewModel> _fields;
}
