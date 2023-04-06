using LiteDB.Studio.Cross.Contracts.DTO;
using LiteDB.Studio.Cross.Contracts.Interfaces;
using LiteDB.Studio.Cross.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LiteDB.Studio.Cross.Services {
    public class ConnectionsManager {
        private readonly Dictionary<string, IConnection> _connections = new();
        private readonly DbConnectionsFabric _connectionsFabric;

        public event Action<string> DatabaseDisconnected;

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

        public bool Disconnect(string id) {
            if (_connections.Remove(id, out var db)) {
                db.Disconnect();
                OnDatabaseDisconnected(id);
                return true;
            }
            return false;
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

        protected virtual void OnDatabaseDisconnected(string id) {
            DatabaseDisconnected?.Invoke(id);
        }
        /// <summary>
        /// Send query to database and get a few items for collection
        /// </summary>
        /// <param name="name">Collection name</param>
        /// <param name="count">Count of items</param>
        /// <param name="id">ConnectionId</param>
        /// <returns><see cref="QueryResultDto"/> if successful or <see langword="null"/></returns>
        public QueryResultDto? GetItems(string? name, int count, string id) {
            if (_connections.TryGetValue(id, out var connection)) {
                var query = $@"SELECT $ FROM {name}";
                if (count > 0) query.Concat($" LIMIT {count}");
                return connection.SendQuery(query);
            }

            return null;
        }
    }
}