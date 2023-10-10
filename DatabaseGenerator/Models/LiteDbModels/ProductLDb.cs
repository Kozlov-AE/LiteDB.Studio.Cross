namespace DatabaseGenerator.Models.LiteDbModels {
    public class ProductLDb {
        public long ProductId { get; set; }
        public string? ProductName { get; set; }
        public long? SupplierId { get; set; }
        public long? CategoryId { get; set; }
        public string? Unit { get; set; }
        public decimal? Price { get; set; }
    }
}