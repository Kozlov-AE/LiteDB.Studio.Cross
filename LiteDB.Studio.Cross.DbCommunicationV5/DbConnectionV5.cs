using LiteDB.Studio.Cross.Contracts.Interfaces;

namespace LiteDB.Studio.Cross.DbCommunicationV5 {
    public class DbConnectionV5 : IConnection {
        private ILiteDatabase _db;

        public DbConnectionV5(ILiteDatabase db) {
            _db = db;
        }
        public void Disconnect() {
            _db.Dispose();
        }

        public string[] GetCollectionNames() {
            throw new NotImplementedException();
        }
    }
}