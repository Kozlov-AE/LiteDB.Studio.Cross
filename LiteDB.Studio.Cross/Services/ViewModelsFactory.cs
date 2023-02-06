using LiteDB.Studio.Cross.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace LiteDB.Studio.Cross.Services {
    public class ViewModelsFactory {
        private readonly IServiceProvider _services;
        
        public ViewModelsFactory(IServiceProvider services) {
            _services = services;
        }

        public ViewModelBase? GetViewModel(Type type) {
            var vm = _services.GetService(type);
            return vm as ViewModelBase;
        }
    }
}