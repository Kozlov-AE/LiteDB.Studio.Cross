using AvaloniaEdit.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LiteDB.Studio.Cross.Models {
    public class ConnectionModel {
        private ILiteDatabase _db;
        private string _dbPath;
        public string Name { get; set; }
        public List<DbCollectionModel> SystemCollections { get; set; } = new List<DbCollectionModel>();
        public List<DbCollectionModel> UserCollections { get; set; } = new List<DbCollectionModel>();
        public bool IsDbConnected;

        public ConnectionModel(ConnectionString cs) {
            try {
                _db = new LiteDatabase(cs);
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
        }
    }
}