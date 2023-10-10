using DatabaseGenerator.Models.LiteDbModels;
using System;
using System.Collections.Generic;

namespace DatabaseGenerator.Models;

public partial class Shipper
{
    public long ShipperId { get; set; }

    public string? ShipperName { get; set; }

    public string? Phone { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    
    public ShipperLDb MapToLDb() {
        return new ShipperLDb() { ShipperId = this.ShipperId, Phone = this.Phone, ShipperName = this.ShipperName };
    }
}
