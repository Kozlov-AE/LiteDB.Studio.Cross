using LiteDB.Studio.Cross.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace LiteDB.Studio.Cross.Services {
    public class OpenDbHistory : IOpenDbHistory {
        private string _savePath = "OpennedHistory.json";
        private Queue<string> _story = new();

        public IEnumerable<string> GetHistory() { return _story; }
        public void AddToStory(string path) {
            if (!_story.Contains(path)) {
                if (_story.Count > 9)
                _story.Dequeue();
                _story.Enqueue(path);
            }
        }
        private void Save() {
            var json = System.Text.Json.JsonSerializer.Serialize(_story, new JsonSerializerOptions() { WriteIndented = true });
            File.WriteAllText(_savePath, json);
        }
        private void Load() {
            if (!File.Exists(_savePath)) { return; }
            var txt = File.ReadAllText(_savePath) ?? string.Empty;
            try {
                _story = System.Text.Json.JsonSerializer.Deserialize<Queue<string>>(txt)!;
            }
            catch (System.Exception) {
                _story = new Queue<string>();
                return;
            }
        }
    }
}
