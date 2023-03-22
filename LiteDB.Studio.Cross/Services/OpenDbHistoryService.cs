using LiteDB.Studio.Cross.Models.EventArgs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace LiteDB.Studio.Cross.Services {
    public class OpenDbHistoryService {
        private string _savePath = "OpennedHistory.json";
        private LinkedList<string> _story = new();

        public OpenDbHistoryService() {
            Load();
        }

        public IEnumerable<string> GetHistory() { return _story; }
        public event EventHandler<OpenDbHistoryEventArgs>? OpenDbHistoryChanged;

        public void AddToStory(string path) {
            var str = _story.FirstOrDefault(s => s == path);
            if (!string.IsNullOrWhiteSpace(str)) {
                _story.Remove(str);
                OpenDbHistoryChanged?.Invoke(this, new OpenDbHistoryEventArgs(OpenDbHistoryEventTypes.PathRemoved, str));
            }
                
            if (_story.Count > 9) {
                var p = _story.Last();
                _story.RemoveLast();
                OpenDbHistoryChanged?.Invoke(this, new OpenDbHistoryEventArgs(OpenDbHistoryEventTypes.PathRemoved, p));
            }

            _story.AddFirst(path);
            OpenDbHistoryChanged?.Invoke(this, new OpenDbHistoryEventArgs(OpenDbHistoryEventTypes.PathAdded, path));
            Save();
        }
        private void Save() {
            var json = System.Text.Json.JsonSerializer.Serialize(_story, new JsonSerializerOptions() { WriteIndented = true });
            File.WriteAllText(_savePath, json);
        }
        private void Load() {
            if (!File.Exists(_savePath)) { return; }
            var txt = File.ReadAllText(_savePath) ?? string.Empty;
            try {
                _story = System.Text.Json.JsonSerializer.Deserialize<LinkedList<string>>(txt)!;
            }
            catch (System.Exception ex) {
                _story = new LinkedList<string>();
            }
        }
        
        
    }
}
