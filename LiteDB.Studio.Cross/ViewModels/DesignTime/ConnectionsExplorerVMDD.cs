using LiteDB.Studio.Cross.Interfaces;
using LiteDB.Studio.Cross.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LiteDB.Studio.Cross.ViewModels.DesignTime {
    public class ConnectionsExplorerVMDD : IConnectionsExplorerViewModel {
        public ObservableCollection<ConnectionModel> Connections { get; set; } =
            new ObservableCollection<ConnectionModel>() {
                new ConnectionModel() {
                    Name = "Connection 1",
                    IsDbConnected = true,
                    SystemCollections = new List<DbCollectionModel>() {
                        new DbCollectionModel() { CollectionName = "$Sys1" },
                        new DbCollectionModel() { CollectionName = "$Sys2" },
                        new DbCollectionModel() { CollectionName = "$Sys3" },
                    },
                    UserCollections = new List<DbCollectionModel>() {
                        new DbCollectionModel() { CollectionName = "My collection 1", Properties = new HashSet<PropertyModel>()},
                        new DbCollectionModel() { CollectionName = "My collection 2", Properties = new HashSet<PropertyModel>()},
                        new DbCollectionModel() { CollectionName = "My collection 3", Properties = new HashSet<PropertyModel>()},
                    }
                },
                new ConnectionModel() {
                    Name = "Connection 2",
                    IsDbConnected = true,
                    SystemCollections = new List<DbCollectionModel>() {
                            new DbCollectionModel() { CollectionName = "$2 - Sys1", Properties = new HashSet<PropertyModel>()},
                            new DbCollectionModel() { CollectionName = "$2 - Sys2", Properties = new HashSet<PropertyModel>()},
                            new DbCollectionModel() { CollectionName = "$2 - Sys3", Properties = new HashSet<PropertyModel>()},
                        },
                    UserCollections = new List<DbCollectionModel>() {
                        new DbCollectionModel() { CollectionName = "2 - My collection 1" },
                        new DbCollectionModel() { CollectionName = "2 - My collection 2" },
                        new DbCollectionModel() { CollectionName = "2 - My collection 3" },
                    }
                }
            };
        public ConnectionModel SelectedConnection { get; set; }
    }
}