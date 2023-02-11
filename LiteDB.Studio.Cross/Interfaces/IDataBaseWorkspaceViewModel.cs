using LiteDB.Studio.Cross.ViewModels;
using System.Collections.ObjectModel;

namespace LiteDB.Studio.Cross.Interfaces
{
    public interface IDataBaseWorkspaceViewModel {
        ObservableCollection<QueryViewModel> Queries { get; set; }
    }
}