namespace LiteDB.Studio.Cross.Contracts.Interfaces {
    public interface IConnection {
        void Disconnect();
        string[] GetCollectionNames();
    }
}