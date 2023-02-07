namespace LiteDB.Studio.Cross.Contracts.Interfaces {
    public interface IConnection {
        /// <summary>
        /// Destroy current connection 
        /// </summary>
        void Disconnect();
        /// <summary>
        /// Get user's collection names
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetCollectionNames();
        /// <summary>
        /// Get system's collection names
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetSystemCollectionNames();
    }
}