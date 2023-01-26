using LiteDB.Studio.Cross.Models.EventArgs;
using System;
using System.Collections.Generic;

namespace LiteDB.Studio.Cross.Interfaces {
    public interface IOpenDbHistoryService {
        void AddToStory(string path);
        IEnumerable<string> GetHistory();
        event EventHandler<OpenDbHistoryEventArgs> OpenDbHistoryChanged;
    }
}