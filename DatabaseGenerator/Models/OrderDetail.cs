using DatabaseGenerator.Models.LiteDbModels;
using System;
using System.Collections.Generic;

namespace DatabaseGenerator.Models;

public partial class OrderDetail
{
    public long OrderDetailId { get; set; }

    public long? OrderId { get; set; }

    public long? ProductId { get; set; }

    public long? Quantity { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }
    public OrderDetailLDb MapToLDb() {
        return new OrderDetailLDb() {
            OrderDetailId = this.OrderDetailId,
            OrderId = this.OrderId,
            ProductId = this.ProductId,
            Quantity = this.Quantity
        };
    }
}
