using LiteDB.Studio.Cross.Contracts.DTO;
using LiteDB.Studio.Cross.Contracts.Interfaces;
using LiteDB.Studio.Cross.DbCommunicationV4;
using LiteDB.Studio.Cross.DbCommunicationV5;
using Microsoft.Extensions.DependencyInjection;

namespace LiteDB.Studio.Cross.Services {
    public class DbConnectionsFabric {
        private readonly ServiceProvider _services;
        public DbConnectionsFabric(ServiceProvider services) {
            _services = services;
        }

        public IConnection? ConnectToDb(ConnectionParametersDto parameters) {
            string version = string.Empty;
            //todo Define the db version;
            var comm = GetCommunicationService(version);
            var db = comm.Connect(parameters);
            return db;
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