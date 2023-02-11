using LiteDB.Studio.Cross.Contracts.DTO;

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
        /// <summary>
        /// Send query to database
        /// </summary>
        /// <param name="query">Query string</param>
        /// <returns>Result</returns>
        QueryResultDto SendQuery(string query);
    }
}