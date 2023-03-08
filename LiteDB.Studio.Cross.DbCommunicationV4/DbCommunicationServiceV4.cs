extern alias LiteDBv4;
using LiteDB.Studio.Cross.Contracts;
using LiteDB.Studio.Cross.Contracts.DTO;
using LiteDB.Studio.Cross.Contracts.Interfaces;

namespace LiteDB.Studio.Cross.DbCommunicationV4;

public class DbCommunicationServiceV4 : IDbCommunicationService {
    public IConnection Connect(ConnectionParametersDto parameters) {
        try {
            var cs = ConfigureConnectionString(parameters);
            var db = new LiteDBv4.LiteDB.LiteDatabase(cs);
            return new DbConnectionV4(db);
        }
        catch (Exception ex) {
            throw;
        }

    }
    
    private LiteDBv4.LiteDB.ConnectionString ConfigureConnectionString(ConnectionParametersDto parameters) {
        var cs = new LiteDBv4.LiteDB.ConnectionString();
        cs.Mode = parameters.IsShared ? LiteDBv4.LiteDB.FileMode.Shared : LiteDBv4.LiteDB.FileMode.Exclusive;
        cs.Mode = parameters.IsReadOnly ? LiteDBv4.LiteDB.FileMode.ReadOnly : cs.Mode;
        cs.Filename = parameters.DbPath;
        cs.Upgrade = parameters.IsUpgrade;
        cs.Password = parameters.Password?.Trim().Length > 0 ? parameters.Password?.Trim() : null;
        if (int.TryParse(parameters.InitSize, out var initSize)) {
            cs.InitialSize = initSize * Constants.Mb;
        }
        return cs;
    }

}