using System.Collections.Generic;

namespace LiteDB.Studio.Cross.Interfaces {
    public interface IOpenDbHistory {
        void AddToStory(string path);
        IEnumerable<string> GetHistory();
    }
}