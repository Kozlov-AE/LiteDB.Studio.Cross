using LiteDB.Studio.Cross.Contracts;
using LiteDB.Studio.Cross.Contracts.DTO;
using LiteDB.Studio.Cross.Contracts.Interfaces;

namespace LiteDB.Studio.Cross.DbCommunicationV5;

public class DbCommunicationServiceV5 : IDbCommunicationService {
    public IConnection? Connect(ConnectionParametersDto parameters) {
        try {
            var db = new LiteDatabase(ConfigureConnectionString(parameters));
            return new DbConnectionV5(db);
        }
        catch (Exception ex) {
            throw;
        }
    }
    
    private ConnectionString ConfigureConnectionString(ConnectionParametersDto parameters) {
        var cs = new ConnectionString();
        cs.Connection = parameters.IsShared ? ConnectionType.Shared : ConnectionType.Direct;
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

            cs.Collation = new Collation(collation);
        }

        return cs;
    }

}