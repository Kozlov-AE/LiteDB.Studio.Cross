extern alias LiteDBv4;
using LiteDB.Studio.Cross.Contracts.DTO;
using LiteDB.Studio.Cross.Contracts.Interfaces;
using LdbV4 = LiteDBv4::LiteDB;

namespace LiteDB.Studio.Cross.DbCommunicationV4
{
    //
    public class DbConnectionV4 : IConnection {
        private readonly LdbV4.LiteDatabase _db;
        public DbConnectionV4(LdbV4.LiteDatabase db) {
            _db = db;
        }
        public void Disconnect() {
            _db.Dispose();
        }

        public IEnumerable<string> GetCollectionNames() {
            return _db.GetCollectionNames().OrderBy(x => x).ToArray();
        }

        public IEnumerable<string> GetSystemCollectionNames() {
            var sc = _db.GetCollection("$cols").FindAll();
            List<string> result = new(15);
            foreach (var doc in sc) {
                result.Add(doc["name"].AsString);
            }
            return result;
        }

        public QueryResultDto SendQuery(string query) {
            throw new NotImplementedException();
        }
    }
}