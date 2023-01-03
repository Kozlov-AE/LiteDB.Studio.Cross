using CommunityToolkit.Mvvm.ComponentModel;

namespace LiteDB.Studio.Cross.ViewModels;

public partial class DbConnectionOptionsViewModel : ViewModelBase
{
    [ObservableProperty] private bool _isDirect;
    [ObservableProperty] private bool _isShared;
    [ObservableProperty] private string? _dbPath;
    [ObservableProperty] private string? _password;
    [ObservableProperty] private string? _initSize;
    [ObservableProperty] private string? _culture;
    [ObservableProperty] private string? _sort;
    [ObservableProperty] private bool _isReadOnly;
    [ObservableProperty] private bool _isUpgrade;
}