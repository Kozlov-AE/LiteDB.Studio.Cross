namespace LiteDB.Studio.Cross.Contracts.DTO {
    public class QueryResultDto {
        public HashSet<string> Fields { get; set; } = new();
        public List<Dictionary<string, dynamic>> Items{ get; set; } = new();
    }
}