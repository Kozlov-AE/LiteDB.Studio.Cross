using LiteDB.Studio.Cross.ViewModels;

namespace LiteDB.Studio.Cross.Interfaces {
    public interface IMainWindowViewModel {
        bool IsLoadDatabaseNeeded { get; set; }
        bool IsDbConnected { get; set; }
        ConnectionParametersViewModel ConnectionOpts { get; set; }
        DatabaseStructureViewModel StructureViewModel  { get; set; }
        
    } 
}