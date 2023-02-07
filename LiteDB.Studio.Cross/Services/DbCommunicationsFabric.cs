using LiteDB.Studio.Cross.Contracts.Interfaces;
using LiteDB.Studio.Cross.DbCommunicationV4;
using LiteDB.Studio.Cross.DbCommunicationV5;
using Microsoft.Extensions.DependencyInjection;

namespace LiteDB.Studio.Cross.Services {
    public class DbCommunicationsFabric {
        private readonly ServiceProvider _services;
        public DbCommunicationsFabric(ServiceProvider services) {
            _services = services;
        }

        public IDbCommunicationService GetCommunicationService(string dbVersion) {
            switch (dbVersion) {
                case "5":
                    return _services.GetRequiredService<DbCommunicationServiceV5>();
                    break;
                case "4":
                    return _services.GetRequiredService<DbCommunicationServiceV4>();
                    break;
            }

            return _services.GetRequiredService<DbCommunicationServiceV5>();
        }
    }
}