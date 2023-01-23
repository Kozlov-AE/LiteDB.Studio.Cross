using LiteDB.Studio.Cross.Models;
using LiteDB.Studio.Cross.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace LiteDB.Studio.Cross.Services {
    public class DatabaseService {
        public DbQuerryResultModel? SendQuery(ILiteDatabase db, string text) {
            var query = text.Replace(Environment.NewLine, " ");
            var doc = new BsonDocument();
            var sql = new StringReader(query);
            IBsonDataReader reader;
            try {
                reader = db.Execute(sql, doc);
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return null;
            }
            using (reader) {
                if (!reader.HasValues) return null;
                DbQuerryResultModel result = new DbQuerryResultModel();
                result.Collection.CollectionName = reader.Collection;
                while (reader.Read()) {
                    CollectionItem model = new CollectionItem();
                    var bson = reader.Current;
                    var docs = bson.AsDocument;
                    try {
                        foreach (var value in docs) {
                            result.Collection.Properties.Add(GetDbValueType(value));
                            dynamic dataVal = null;
                            var val = value.Value;
                            if (val.IsDateTime) dataVal = val.AsDateTime;
                            else if (val.IsBoolean) dataVal = val.AsBoolean;
                            else if (val.IsDecimal) dataVal = val.AsDecimal;
                            else if (val.IsDouble) dataVal = val.AsDouble;
                            else if (val.IsInt32) dataVal = val.AsInt32;
                            else if (val.IsInt64) dataVal = val.AsInt64;
                            else if (val.IsString) dataVal = val.AsString;
                            else if (val.IsBinary) dataVal = val.AsBinary;
                            else dataVal = val.ToString();

                            model.Values.Add(value.Key, dataVal);
                        }
                        result.Items.Add(model);
                    }
                    catch (Exception ex) {
                        Console.WriteLine(ex);
                    }
                }
                return result;
            }
        }

        private PropertyModel GetDbValueType(KeyValuePair<string, BsonValue> pair) {
            Type type;
            var value = pair.Value;
            if (value.IsString) type = typeof(string);
            else if (value.IsBoolean) type = typeof(bool);
            else if (value.IsBinary) type = typeof(byte[]);
            else if (value.IsDecimal) type = typeof(decimal);
            else if (value.IsDouble) type = typeof(double);
            else if (value.IsInt32) type = typeof(int);
            else if (value.IsInt64) type = typeof(long);
            else if (value.IsDateTime) type = typeof(DateTime);
            else type = typeof(string);
            return new PropertyModel(pair.Key, type);
        }
        
    }
}