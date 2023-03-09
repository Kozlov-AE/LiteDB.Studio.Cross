extern alias LiteDBv5;
using LiteDB.Studio.Cross.Contracts;
using LiteDB.Studio.Cross.Contracts.DTO;
using LiteDB.Studio.Cross.Contracts.Interfaces;
using LdbV5 = LiteDBv5::LiteDB;

namespace LiteDB.Studio.Cross.DbCommunicationV5;

public class DbCommunicationServiceV5 : IDbCommunicationService {
    public IConnection? Connect(ConnectionParametersDto parameters) {
        try {
            var db = new LdbV5.LiteDatabase(ConfigureConnectionString(parameters));
            return new DbConnectionV5(db);
        }
        catch (Exception ex) {
            throw;
        }
    }
    
    private LdbV5.ConnectionString ConfigureConnectionString(ConnectionParametersDto parameters) {
        var cs = new LdbV5.ConnectionString();
        cs.Connection = parameters.IsShared ? LdbV5.ConnectionType.Shared : LdbV5.ConnectionType.Direct;
        cs.Filename = parameters.DbPath;
        cs.ReadOnly = parameters.IsReadOnly;
        cs.Upgrade = parameters.IsUpgrade;
        cs.Password = parameters.Password?.Trim().Length > 0 ? parameters.Password?.Trim() : null;
        if (int.TryParse(parameters.InitSize, out var initSize)) {
            cs.InitialSize = initSize * Constants.Mb;
        }

        if (!string.IsNullOrEmpty(parameters.Culture)) {
            var collation = parameters.Culture;
            if (!string.IsNullOrEmpty(parameters.Sort)) {
                collation += "/" + parameters.Sort;
            }

            cs.Collation = new LdbV5.Collation(collation);
        }

        return cs;
    }

}