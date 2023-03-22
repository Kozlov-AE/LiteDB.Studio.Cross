using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;


namespace LiteDB.Studio.Cross.ViewModels;
public partial class CollectionViewModel: ViewModelBase {
    [ObservableProperty] private string _name;
    [ObservableProperty] private ObservableCollection<DbCollectionFieldViewModel> _fields;

    public CollectionViewModel(string name) {
        Fields = new ObservableCollection<DbCollectionFieldViewModel>();
        Name = name;
    }
    public event EventHandler<int> OnAskedLoadItemsEvent; 

    [RelayCommand]
    private void LoadItems(int count) {
        OnAskedLoadItemsEvent?.Invoke(this, count);
    }
}
