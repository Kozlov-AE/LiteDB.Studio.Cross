using LiteDB.Studio.Cross.Interfaces;
using LiteDB.Studio.Cross.Services;
using LiteDB.Studio.Cross.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteDB.Studio.Cross {
    public static class IoC {
        public static IServiceProvider ConfigureServices(){
            var services = new ServiceCollection();
            services.AddSingleton<IOpenDbHistory, OpenDbHistory>();
            services.AddSingleton<MainWindowViewModel>();

            return services.BuildServiceProvider();
        }
    }
}
