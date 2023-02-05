using System.Collections.Generic;
using LiteDB.Studio.Cross.Contracts.DTO;
using System;
using System.Security.AccessControl;

namespace LiteDB.Studio.Cross.Services {
    public class DataBaseConnectionsManagerService {
        private Dictionary<string, ILiteDbConnection> _connections;

        public DataBaseDto Connect(ConnectionParametersDto parameters) {
            throw new NotImplementedException();
        }

        public void Disconnect(string id) {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Send query to DB
        /// </summary>
        /// <param name="id">Connection id</param>
        /// <returns>Null if query unsuccessful</returns>
        QueryResultDto? SendQuery(string id, string query) {
            if (_connections.TryGetValue(id, out var conn)) {
                return conn.SendQuery(query);
            }
            else {
                return null;
            }
        }
    }

    public interface ILiteDbConnection {
        void Disconnect();
        QueryResultDto SendQuery(string query);
    }
}
