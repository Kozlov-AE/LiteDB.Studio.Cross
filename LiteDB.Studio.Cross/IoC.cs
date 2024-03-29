﻿using LiteDB.Studio.Cross.Contracts.Interfaces;
using LiteDB.Studio.Cross.DbCommunicationV5;
using LiteDB.Studio.Cross.Services;
using LiteDB.Studio.Cross.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace LiteDB.Studio.Cross {
    public static class IoC {
        public static IServiceProvider ConfigureServices(){
            var services = new ServiceCollection();
            try {
                services.AddSingleton<ConnectionsManager>();
                services.AddSingleton<IDbCommunicationService, DbCommunicationServiceV5>();
                services.AddTransient<DbConnectionsFabric>();
                services.AddSingleton<ViewModelsFactory>();
                services.AddSingleton<OpenDbHistoryService>();
                
                services.AddTransient<DataBaseWorkspaceViewModel>();
                services.AddTransient<DatabaseViewModel>();
                services.AddTransient<ConnectionParametersViewModel>();
                services.AddTransient<QueryViewModel>();
                services.AddSingleton<DataBaseExplorerViewModel>();
                services.AddSingleton<MainWindowViewModel>();
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
            }

            return services.BuildServiceProvider();
        }
    }

}
