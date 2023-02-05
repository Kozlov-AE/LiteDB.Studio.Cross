using CommunityToolkit.Mvvm.Collections;
using LiteDB.Studio.Cross.Interfaces;
using LiteDB.Studio.Cross.Models;
using System.Collections.ObjectModel;

namespace LiteDB.Studio.Cross.ViewModels.DesignTime {
    public class MainWindowVMDD : IMainWindowViewModel {
        public bool IsLoadDatabaseNeeded { get; set; } = false;
        public bool IsDbConnected { get; set; } = true;
        public ConnectionParametersViewModel ConnectionOpts { get; set; }

        public DatabaseStructureViewModel StructureViewModel { get; set; } = new DatabaseStructureViewModel() {
            DbName = "MyDatabase",
            SysDirectory = new DatabaseStructureViewModel() {
                DbName = "System",
                Collections = new ObservableCollection<DbCollectionViewModel>() {
                    new DbCollectionViewModel() {
                        CollectionName = "$colls", Fields = new ObservableCollection<PropertyModel>() {
                            new PropertyModel("f1", typeof(string)),
                            new PropertyModel("f2", typeof(int)),
                            new PropertyModel("f3", typeof(bool))
                        }
                    },
                    new DbCollectionViewModel() {
                        CollectionName = "$params", Fields = new ObservableCollection<PropertyModel>() {
                            new PropertyModel("f1", typeof(string)),
                            new PropertyModel("f2", typeof(int)),
                            new PropertyModel("f3", typeof(bool))
                        }
                    },
                    new DbCollectionViewModel() {
                        CollectionName = "opts", Fields = new ObservableCollection<PropertyModel>() {
                            new PropertyModel("f1", typeof(string)),
                            new PropertyModel("f2", typeof(int)),
                            new PropertyModel("f3", typeof(bool))
                        }
                    }
                }
            },
            Collections = new ObservableCollection<DbCollectionViewModel>() {
                new DbCollectionViewModel() {
                    CollectionName = "Collection 1", Fields = new ObservableCollection<PropertyModel>() {
                        new PropertyModel("C1F1", typeof(string)),
                        new PropertyModel("C1F2", typeof(int)),
                        new PropertyModel("C1F3", typeof(string)),
                        new PropertyModel("C1F4", typeof(bool))
                    }
                },
                new DbCollectionViewModel() {
                    CollectionName = "Collection 2", Fields = new ObservableCollection<PropertyModel>() {
                        new PropertyModel("C2F1", typeof(bool)),
                        new PropertyModel("C3F2", typeof(int)),
                        new PropertyModel("C3F3", typeof(double)),
                        new PropertyModel("C3F4", typeof(decimal))
                    }
                }

            }
        };
    }
}