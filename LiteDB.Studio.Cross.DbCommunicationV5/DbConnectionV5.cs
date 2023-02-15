using LiteDB.Engine;
using LiteDB.Studio.Cross.Contracts.DTO;
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
        public QueryResultDto SendQuery(string text) {
            var query = text.Replace(Environment.NewLine, " ");
            var doc = new BsonDocument();
            var sql = new StringReader(query);
            IBsonDataReader reader;
            try {
                reader = _db.Execute(sql, doc);
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return null;
            }

            QueryResultDto result;
            using (reader) {
                if (!reader.HasValues) return null;
                result = new ();
                while (reader.Read()) {
                    Dictionary<string, dynamic> item = new ();
                    var bson = reader.Current;
                    var docs = bson.AsDocument;
                    foreach (var value in docs) {
                        result.Fields.Add(value.Key);
                        dynamic dataVal = null;
                        var val = value.Value;
                        if (val.IsString) dataVal = val.AsString;
                        else if (val.IsDateTime) dataVal = val.AsDateTime;
                        else if (val.IsBoolean) dataVal = val.AsBoolean;
                        else if (val.IsDecimal) dataVal = val.AsDecimal;
                        else if (val.IsDouble) dataVal = val.AsDouble;
                        else if (val.IsInt32) dataVal = val.AsInt32;
                        else if (val.IsInt64) dataVal = val.AsInt64;
                        else if (val.IsBinary) dataVal = val.AsBinary;
                        else dataVal = val.ToString();
                        item.Add(value.Key, dataVal);
                    }
                    result.Items.Add(item);
                }
            }
            return result;
        }
    }
}