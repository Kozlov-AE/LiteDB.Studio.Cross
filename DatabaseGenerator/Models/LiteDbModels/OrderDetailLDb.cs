namespace DatabaseGenerator.Models.LiteDbModels {
    public class OrderDetailLDb {
        public long OrderDetailId { get; set; }
        public long? OrderId { get; set; }
        public long? ProductId { get; set; }
        public long? Quantity { get; set; }
    }
}