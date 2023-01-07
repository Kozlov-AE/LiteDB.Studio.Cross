using LiteDB.Studio.Cross.Interfaces;
using System.Collections.ObjectModel;

namespace LiteDB.Studio.Cross.ViewModels.DesignTime {
    public class MainWindowVMDD : IMainWindowViewModel {
        public bool IsLoadDatabaseNeeded { get; set; } = false;
        public bool IsDbConnected { get; set; } = true;
        public DbConnectionOptionsViewModel ConnectionOpts { get; set; }

        public DatabaseStructureViewModel StructureViewModel { get; set; } = new DatabaseStructureViewModel() {
            DbName = "MyDatabase",
            SysDirectory = new DatabaseStructureViewModel() {
                DbName = "System",
                Collections = new ObservableCollection<DbCollectionViewModel>() {
                    new DbCollectionViewModel() {
                        CollectionName = "$colls", Fields = new ObservableCollection<string>() { "f1", "f2", "f3" }
                    },
                    new DbCollectionViewModel() {
                        CollectionName = "$params", Fields = new ObservableCollection<string>() { "f1", "f2", "f3" }
                    },
                    new DbCollectionViewModel() {
                        CollectionName = "opts", Fields = new ObservableCollection<string>() { "f1", "f2", "f3" }
                    }
                }
            },
            Collections = new ObservableCollection<DbCollectionViewModel>() {
                new DbCollectionViewModel() {
                    CollectionName = "Collection 1", Fields = new ObservableCollection<string>() {
                        "C1F1","C1F2","C1F3","C1F3"
                    }
                },
                new DbCollectionViewModel() {
                    CollectionName = "Collection 2", Fields = new ObservableCollection<string>() {
                        "C2F1","C2F2","C2F3","C2F3"
                    }
                }

            }
        };
    }
}