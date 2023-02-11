using CommunityToolkit.Mvvm.ComponentModel;
using LiteDB.Studio.Cross.Interfaces;
using System.Collections.ObjectModel;

namespace LiteDB.Studio.Cross.ViewModels;
public partial class DatabaseViewModel: ViewModelBase {
    [ObservableProperty] private string _id;
    [ObservableProperty] private string _name;
    [ObservableProperty] private CollectionSetViewModel _sysCollections;
    [ObservableProperty] private CollectionSetViewModel _dbCollections;
    [ObservableProperty] private IDataBaseWorkspaceViewModel _workspace;
}