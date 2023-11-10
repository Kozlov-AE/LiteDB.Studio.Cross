using LiteDB.Studio.Cross.Contracts.DTO;
using LiteDB.Studio.Cross.Contracts.Enums;
using LiteDB.Studio.Cross.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace LiteDB.Studio.Cross.Services {
    public class ViewModelsFactory {
        private readonly IServiceProvider _services;
        private readonly ConnectionParametersViewModel _connectionParametersVm;
        private readonly ConnectionsManager _connectionsManager;

        public ViewModelsFactory(IServiceProvider services,
                                ConnectionParametersViewModel connectionParametersViewModel,
                                ConnectionsManager connectionsManager) {
            _services = services;
            _connectionParametersVm = connectionParametersViewModel;
            _connectionsManager = connectionsManager;
            
        }

        public ConnectionParametersViewModel GetConnectionParametersViewModel() => _connectionParametersVm;
        
        public ViewModelBase? GetViewModel(Type type) {
            var vm = _services.GetService(type);
            return vm as ViewModelBase;
        }

        public DatabaseViewModel? GetDatabaseViewModel(DataBaseDto dbDto) {
            var result = new DatabaseViewModel(_connectionsManager, dbDto, this) { 
                SysCollections = GetCollectionSetViewModel(DbCollectionTypes.SystemCollection, dbDto.SysCollections), 
                DbCollections = GetCollectionSetViewModel(DbCollectionTypes.DbCollections, dbDto.DbCollections),
                Workspace = new DataBaseWorkspaceViewModel(),
            };
            return result;
        }

        public CollectionSetViewModel GetCollectionSetViewModel(DbCollectionTypes type) {
            var result = new CollectionSetViewModel();
            switch (type) {
                case DbCollectionTypes.SystemCollection:
                    result.Name = "SystemCollections";
                    break;
                case DbCollectionTypes.DbCollections:
                    result.Name = "User collections";
                    break;
            }

            return result;
        }
        
        public CollectionSetViewModel GetCollectionSetViewModel(DbCollectionTypes type, IEnumerable<string> collections) {
            var result = GetCollectionSetViewModel(type);
            foreach (var collection in collections) {
                result.Collections.Add(new CollectionViewModel(collection));
            }

            return result;
        }

        public DataBaseWorkspaceViewModel GetDataBaseWorkspaceViewModel() => new();
    }
}