using LiteDB.Studio.Cross.Contracts.Interfaces;

namespace LiteDB.Studio.Cross.DbCommunicationV5 {
    public class DbConnectionV5 : IConnection {
        private readonly ILiteDatabase _db;

        public DbConnectionV5(ILiteDatabase db) {
            _db = db;
        }
        public void Disconnect() {
            _db.Dispose();
        }

        public IEnumerable<string> GetCollectionNames() {
            return _db.GetCollectionNames().OrderBy(x => x).ToArray();
        }
        public IEnumerable<string>  GetSystemCollectionNames() {
            var sc = _db.GetCollection("$cols")
                .Query()
                .Where("type = 'system'")
                .OrderBy("name")
                .ToDocuments();
            List<string> result = new(15);
            foreach (var doc in sc) {
                result.Add(doc["name"].AsString);
            }
            return result;
        }
    }
}