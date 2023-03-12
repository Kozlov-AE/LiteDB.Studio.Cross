using LiteDB.Studio.Cross.Contracts.DTO;
using LiteDB.Studio.Cross.Interfaces;
using LiteDB.Studio.Cross.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Runtime.InteropServices;
using Tmds.DBus;

namespace LiteDB.Studio.Cross.Services {
    public class ViewModelsFactory {
        private readonly IServiceProvider _services;
        
        public ViewModelsFactory(IServiceProvider services) {
            _services = services;
        }

        public IDataBaseWorkspaceViewModel GetWorkspaceVm(string id) {
            var vm = _services.GetRequiredService<DataBaseWorkspaceViewModel>();
            vm.SetConnectionId(id);
            vm.AddQueryModel();
            return vm;
        }

        public DatabaseViewModel GetDatabaseViewModel(DataBaseDto dto) {
            var vm = _services.GetRequiredService<DatabaseViewModel>();
            vm.Id = dto.Id;
            vm.Name = dto.Name;
            vm.DbCollections = new CollectionSetViewModel(){Name = "UserCollections"};
            foreach (var name in dto.DbCollections) {
                vm.DbCollections.Collections.Add(new CollectionViewModel{Name = name});
            }
            vm.SysCollections = new CollectionSetViewModel(){Name = "UserCollections"};
            foreach (var name in dto.SysCollections) {
                vm.SysCollections.Collections.Add(new CollectionViewModel{Name = name});
            }
            vm.Workspace = GetWorkspaceVm(dto.Id);
            return vm;
        }

        public QueryViewModel GetQueryVm(string name, string connetionId) {
            var vm = _services.GetRequiredService<QueryViewModel>();
            vm.Name = name;
            vm.SetConnectionId(connetionId);
            return vm;
        }

        public ConnectionParametersViewModel GetConnectionParametersViewModel(){
            return _services.GetRequiredService<ConnectionParametersViewModel>();
        }
        
        public ViewModelBase? GetViewModel(Type type) {
            var vm = _services.GetService(type);
            return vm as ViewModelBase;
        }
    }
}