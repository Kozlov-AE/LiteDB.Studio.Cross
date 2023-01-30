using CommunityToolkit.Mvvm.ComponentModel;

namespace LiteDB.Studio.Cross.ViewModels {
    public partial class RightSideTabViewModel : ViewModelBase {
        [ObservableProperty] private QueryResultViewModel? _resultVm;
    }
}