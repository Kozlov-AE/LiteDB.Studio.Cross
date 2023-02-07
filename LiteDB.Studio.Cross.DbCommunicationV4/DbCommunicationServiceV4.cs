using LiteDB.Studio.Cross.Contracts.DTO;
using LiteDB.Studio.Cross.Contracts.Interfaces;

namespace LiteDB.Studio.Cross.DbCommunicationV4;

public class DbCommunicationServiceV4 : IDbCommunicationService {
    public IConnection Connect(ConnectionParametersDto parameters) {
        throw new NotImplementedException();
    }
}