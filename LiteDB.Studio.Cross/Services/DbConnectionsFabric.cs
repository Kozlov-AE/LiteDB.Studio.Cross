using LiteDB.Studio.Cross.Contracts.DTO;
using LiteDB.Studio.Cross.Contracts.Interfaces;
using LiteDB.Studio.Cross.DbCommunicationV4;
using LiteDB.Studio.Cross.DbCommunicationV5;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace LiteDB.Studio.Cross.Services {
    public class DbConnectionsFabric {
        private readonly ServiceProvider _services;
        private readonly string[] _versions = { "5", "4" };
        public DbConnectionsFabric(ServiceProvider services) {
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

        private IDbCommunicationService GetCommunicationService(string dbVersion) {
            switch (dbVersion) {
                case "5":
                    return _services.GetRequiredService<DbCommunicationServiceV5>();
                case "4":
                    return _services.GetRequiredService<DbCommunicationServiceV4>();
            }

            return _services.GetRequiredService<DbCommunicationServiceV5>();
        }
    }
}