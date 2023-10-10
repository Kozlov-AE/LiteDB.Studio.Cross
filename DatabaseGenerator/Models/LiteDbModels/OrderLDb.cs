namespace DatabaseGenerator.Models.LiteDbModels {
    public class OrderLDb {
        public long OrderId { get; set; }
        public long? CustomerId { get; set; }
        public long? EmployeeId { get; set; }
        public DateTime? OrderDate { get; set; }
        public long? ShipperId { get; set; }
    }
}