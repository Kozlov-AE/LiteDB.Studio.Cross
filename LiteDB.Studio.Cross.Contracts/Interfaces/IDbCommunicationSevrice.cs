using LiteDB.Studio.Cross.Contracts.DTO;

namespace LiteDB.Studio.Cross.Contracts.Interfaces {
    public interface IDbCommunicationService {
        IConnection? Connect(ConnectionParametersDto parameters);
    }
}