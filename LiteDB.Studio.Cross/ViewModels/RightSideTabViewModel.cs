using CommunityToolkit.Mvvm.ComponentModel;
using LiteDB.Studio.Cross.Models;

namespace LiteDB.Studio.Cross.ViewModels {
    public partial class RightSideTabViewModel : ViewModelBase {
        private DbQuerryResultModel _resultModel;
        [ObservableProperty] private QueryResultViewModel? _resultVm;
    }
}