using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace LiteDB.Studio.Cross.ViewModels;
public partial class DbCollectionFieldViewModel: ViewModelBase {
    [ObservableProperty] private string _name;
    [ObservableProperty] private string _fType;

    public DbCollectionFieldViewModel() {
        Name = String.Empty;
        FType = String.Empty;
    }
}
