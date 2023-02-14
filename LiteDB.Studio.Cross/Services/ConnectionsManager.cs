using LiteDB.Studio.Cross.Contracts.DTO;
using LiteDB.Studio.Cross.Contracts.Interfaces;
using LiteDB.Studio.Cross.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace LiteDB.Studio.Cross.Services {
    public class ConnectionsManager {
        private readonly Dictionary<string, IConnection> _connections = new();
        private readonly DbConnectionsFabric _connectionsFabric;

        public ConnectionsManager(DbConnectionsFabric connectionsFabric) {
            _connectionsFabric = connectionsFabric;
        }

        public DataBaseDto? Connect(ConnectionParametersDto connectionString) {
            if (_connections.ContainsKey(connectionString.DbPath)) return null;
            var result = new DataBaseDto();
            try {
                var db = _connectionsFabric.GetConnection(connectionString);
                if (db == null) return null;
                _connections.Add(connectionString.DbPath, db);
                result.Id = connectionString.DbPath;
                result.Name = Path.GetFileName(connectionString.DbPath);
                result.DbCollections = db.GetCollectionNames();
                result.SysCollections = db.GetSystemCollectionNames();
                return result;
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return null;
            }
        }
        public List<DbCollectionModel>? GetCollections(string fileName) {
            if (_connections.TryGetValue(fileName, out var conn)) {
                var collects = conn.GetCollectionNames();
                throw new NotImplementedException("GetCollections not implemented");
            }
            return null;
        }

        //todo Change from null to custom exception
        public QueryResultDto? SendQuery(string connectionId, string text) {
            if (_connections.TryGetValue(connectionId, out var conn)) {
                return conn.SendQuery(text);
            }
            return null;
        }
        
    }
}