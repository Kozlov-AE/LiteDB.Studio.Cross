using LiteDB.Studio.Cross.Interfaces;
using LiteDB.Studio.Cross.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteDB.Studio.Cross {
    public static class IoC {
        public static IServiceProvider Services { get; }
        public static IServiceProvider ConfigureServices(){
            var services = new ServiceCollection();
            services.AddSingleton<ILastOpenedDbStoryService, LastOpenedDbStoryService>();

            return services.BuildServiceProvider();
        }
    }
}
