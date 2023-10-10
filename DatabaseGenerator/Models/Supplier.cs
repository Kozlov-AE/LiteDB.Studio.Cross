using DatabaseGenerator.Models.LiteDbModels;
using System;
using System.Collections.Generic;

namespace DatabaseGenerator.Models;

public partial class Supplier
{
    public long SupplierId { get; set; }

    public string? SupplierName { get; set; }

    public string? ContactName { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? PostalCode { get; set; }

    public string? Country { get; set; }

    public string? Phone { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    
    public SupplierLDb MapToLDb() {
        return new SupplierLDb() {
            SupplierId = this.SupplierId,
            SupplierName = this.SupplierName,
            ContactName = this.ContactName,
            Address = this.Address,
            City = this.City,
            PostalCode = this.PostalCode,
            Country = this.Country,
            Phone = this.Phone
        };
    }
}
