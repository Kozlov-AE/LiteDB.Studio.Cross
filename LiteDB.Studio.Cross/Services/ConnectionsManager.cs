using LiteDB.Studio.Cross.Models;
using System;
using System.Collections.Generic;

namespace LiteDB.Studio.Cross.Services {
    public class ConnectionsManager {
        private Dictionary<string, ILiteDatabase> _connections;

        public string AddConnection(ConnectionString connectionString) {
            if (_connections.ContainsKey(connectionString.Filename)) return string.Empty;
            try {
                var db = new LiteDatabase(connectionString);
                _connections.Add(connectionString.Filename, db);
                return connectionString.Filename;
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return string.Empty;
            }
        }

        public List<DbCollectionModel>? GetCollections(string fileName) {
            if (_connections.TryGetValue(fileName, out var conn)) {
                var collects = conn.GetCollectionNames();
            }
        }
        
    }
}