using LiteDB.Studio.Cross.Contracts;
using LiteDB.Studio.Cross.Contracts.DTO;
using LiteDB.Studio.Cross.Contracts.Interfaces;

namespace LiteDB.Studio.Cross.DbCommunicationV4;

public class DbCommunicationServiceV4 : IDbCommunicationService {
    public IConnection Connect(ConnectionParametersDto parameters) {
        try {
            var db = new LiteDatabase(ConfigureConnectionString(parameters));
            return new DbConnectionV4(db);
        }
        catch (Exception ex) {
            throw;
        }

    }
    
    private ConnectionString ConfigureConnectionString(ConnectionParametersDto parameters) {
        var cs = new ConnectionString();
        cs.Mode = parameters.IsShared ? FileMode.Shared : FileMode.Exclusive;
        cs.Mode = parameters.IsReadOnly ? FileMode.ReadOnly : cs.Mode;
        cs.Filename = parameters.DbPath;
        cs.Upgrade = parameters.IsUpgrade;
        cs.Password = parameters.Password?.Trim().Length > 0 ? parameters.Password?.Trim() : null;
        if (int.TryParse(parameters.InitSize, out var initSize)) {
            cs.InitialSize = initSize * Constants.Mb;
        }
        return cs;
    }

}