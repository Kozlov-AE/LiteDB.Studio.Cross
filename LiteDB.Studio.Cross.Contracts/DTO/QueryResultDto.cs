namespace LiteDB.Studio.Cross.Contracts.DTO {
    public class QueryResultDto {
        public HashSet<string> Fields { get; set; } = new();
        public List<QueryResulCollectionItem> Items { get; set; } = new();
    }

    public class QueryResulCollectionItem {
        public List<QueryResulCollectionItemCell> Cells { get; set; } = new();
    }

    public record QueryResulCollectionItemCell(string Name, dynamic Value);
}