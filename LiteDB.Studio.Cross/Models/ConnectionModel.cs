using AvaloniaEdit.Utils;
using LiteDB.Studio.Cross.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Linq;

namespace LiteDB.Studio.Cross.Models {
    public class ConnectionModel {
        private ILiteDatabase _db;
        private string _fileName;

        private readonly DatabaseService_OLD _databaseServiceOld = new DatabaseService_OLD();
        public string Name { get; set; }
        public List<DbCollectionModel> SystemCollections { get; set; } = new List<DbCollectionModel>();
        public List<DbCollectionModel> UserCollections { get; set; } = new List<DbCollectionModel>();
        public bool IsDbConnected;

        public string FileName => _fileName;

        public bool Connect(ConnectionString cs) {
            try {
                _db = new LiteDatabase(cs);
                _fileName = cs.Filename;
                Name = Path.GetFileName(cs.Filename);

                var sc = _db.GetCollection("$cols")
                    .Query()
                    .Where("type = 'system'")
                    .OrderBy("name")
                    .ToDocuments();
                foreach (var doc in sc) {
                    var collection = new DbCollectionModel();
                    collection.CollectionName = doc["name"].AsString;
                    SystemCollections.Add(collection);
                }

                var colls = _db.GetCollectionNames().OrderBy(x => x);
                foreach (var name in colls) {
                    var coll = new DbCollectionModel();
                    coll.CollectionName = name;
                    UserCollections.Add(coll);
                }

                IsDbConnected = true;
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
                IsDbConnected = false;
            }

            return IsDbConnected;
        }
        public bool Disconnect() {
            _db.Dispose();
            IsDbConnected = false;
            return !IsDbConnected;
        }

        public DbQuerryResultModel? SendQuery(string text) {
            var result =  _databaseServiceOld.SendQuery(_db, text);
            if (result == null) return null;
            var coll = UserCollections.FirstOrDefault(c => c.CollectionName == result.Collection.CollectionName);
            if (coll == null) coll = result.Collection;
            coll.Properties.AddRange(result.Collection.Properties.Where(p => coll.Properties.Any(cp => cp.Name == p.Name)));
            return result;
        }

    }
}