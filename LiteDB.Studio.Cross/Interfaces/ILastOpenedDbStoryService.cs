using System.Collections.Generic;

namespace LiteDB.Studio.Cross.Interfaces {
    public interface ILastOpenedDbStoryService {
        void AddToStory(string path);
        IEnumerable<string> GetStory();
    }
}