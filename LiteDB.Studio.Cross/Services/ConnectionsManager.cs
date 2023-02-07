using LiteDB.Studio.Cross.Contracts.DTO;
using LiteDB.Studio.Cross.Contracts.Interfaces;
using LiteDB.Studio.Cross.Models;
using System;
using System.Collections.Generic;

namespace LiteDB.Studio.Cross.Services {
    public class ConnectionsManager {
        private Dictionary<string, IConnection> _connections;
        private readonly DbCommunicationsFabric _communicationsFabric;

        public ConnectionsManager(DbCommunicationsFabric communicationsFabric) {
            _communicationsFabric = communicationsFabric;
        }

        public string AddConnection(ConnectionParametersDto connectionString) {
            if (_connections.ContainsKey(connectionString.DbPath)) return string.Empty;
            try {
                string version = string.Empty;
                //todo Define the db version;
                var comm = _communicationsFabric.GetCommunicationService(version);
                var db = comm.Connect(connectionString);
                _connections.Add(connectionString.DbPath, db);
                return connectionString.DbPath;
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return string.Empty;
            }
        }

        public List<DbCollectionModel>? GetCollections(string fileName) {
            if (_connections.TryGetValue(fileName, out var conn)) {
                var collects = conn.GetCollectionNames();
                throw new NotImplementedException("GetCollections not implemented");
            }
            else return null;
        }
        
    }
}