using AvaloniaEdit.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using LiteDB.Studio.Cross.Interfaces;
using LiteDB.Studio.Cross.Models.EventArgs;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace LiteDB.Studio.Cross.ViewModels {
    public partial class ConnectionParametersViewModel : ViewModelBase {
        private IOpenDbHistoryService _historyService;
        
        [ObservableProperty] private ObservableCollection<string> _openDbHistory;
        [ObservableProperty] private string _selectedHistoryItem;

        [ObservableProperty] private bool _isDirect;
        [ObservableProperty] private bool _isShared;
        [ObservableProperty] private string? _dbPath;
        [ObservableProperty] private string? _password;
        [ObservableProperty] private string? _initSize;
        [ObservableProperty] private string? _culture;
        [ObservableProperty] private string? _sort;
        [ObservableProperty] private bool _isReadOnly;
        [ObservableProperty] private bool _isUpgrade;

        public ConnectionParametersViewModel(IOpenDbHistoryService historyService) {
            _historyService = historyService;
            _historyService.OpenDbHistoryChanged += HistoryServiceOnOpenDbHistoryChanged;
            OpenDbHistory = new ObservableCollection<string>();
            OpenDbHistory.AddRange(historyService.GetHistory());
            this.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e) {
            switch (e.PropertyName) {
                case "SelectedHistoryItem":
                    if (!string.IsNullOrEmpty(SelectedHistoryItem))
                        DbPath = SelectedHistoryItem;
                    break;
                case "DbPath":
                    SelectedHistoryItem = string.Empty;
                    break;
            }
        }

        private void HistoryServiceOnOpenDbHistoryChanged(object? sender, OpenDbHistoryEventArgs e) {
            switch (e.EventType) {
                case OpenDbHistoryEventTypes.PathAdded:
                    OpenDbHistory.Add(e.ChangedPath);
                    break;
                case OpenDbHistoryEventTypes.PathRemoved:
                    var p = OpenDbHistory.FirstOrDefault(p => p == e.ChangedPath);
                    if (!string.IsNullOrWhiteSpace(p)) OpenDbHistory.Remove(p);
                    break;
            }
        }
    }
}