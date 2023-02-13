using LiteDB.Studio.Cross.Contracts.DTO;
using LiteDB.Studio.Cross.Contracts.Interfaces;
using LiteDB.Studio.Cross.DbCommunicationV4;
using LiteDB.Studio.Cross.DbCommunicationV5;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace LiteDB.Studio.Cross.Services {
    public class DbConnectionsFabric {
        private readonly IServiceProvider _services;
        private readonly string[] _versions = { "5", "4" };
        public DbConnectionsFabric(IServiceProvider services) {
            _services = services;
        }

        public IConnection? GetConnection(ConnectionParametersDto parameters) {
            foreach (var version in _versions) {
                try {
                    var comm = GetCommunicationService(version);
                    var db = comm.Connect(parameters);
                    return db;
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                }
            }

            return null;
        }

        private IDbCommunicationService? GetCommunicationService(string dbVersion) {
            var services = _services.GetServices<IDbCommunicationService>();
            switch (dbVersion) {
                case "5":
                    return services.FirstOrDefault(s => s is DbCommunicationServiceV5);
                case "4":
                    return services.FirstOrDefault(s => s is DbCommunicationServiceV4);
            }

            return services.FirstOrDefault(s => s is DbCommunicationServiceV5);
        }
    }
}