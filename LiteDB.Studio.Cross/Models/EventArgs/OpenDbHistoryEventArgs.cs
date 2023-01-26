namespace LiteDB.Studio.Cross.Models.EventArgs {
    public record  OpenDbHistoryEventArgs(OpenDbHistoryEventTypes EventType, string ChangedPath);

    public enum OpenDbHistoryEventTypes {
        PathAdded, PathRemoved
    }
}