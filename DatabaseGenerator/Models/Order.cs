using DatabaseGenerator.Models.LiteDbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseGenerator.Models;

public partial class Order
{
    public long OrderId { get; set; }

    public long? CustomerId { get; set; }

    public long? EmployeeId { get; set; }

    public byte[]? OrderDate { get; set; }

    public long? ShipperId { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Shipper? Shipper { get; set; }
    
    public OrderLDb MapToLDb() {
        return new OrderLDb() {
            OrderId = this.OrderId,
            OrderDate =
                DateTime.Parse(
                    Encoding.Default.GetString(OrderDate?.Where(x => x != 0).ToArray() ?? Array.Empty<byte>())),
            CustomerId = this.CustomerId,
            EmployeeId = this.EmployeeId,
            ShipperId = this.ShipperId
        };
    }
}
