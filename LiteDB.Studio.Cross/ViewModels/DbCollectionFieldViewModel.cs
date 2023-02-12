using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;

namespace LiteDB.Studio.Cross.ViewModels;
public partial class DbCollectionFieldViewModel: ViewModelBase {
    [ObservableProperty] private string _name;
    [ObservableProperty] private string _fType;

    public DbCollectionFieldViewModel() {
        Name = String.Empty;
        FType = String.Empty;
    }
}
