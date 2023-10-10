using DatabaseGenerator.Models.LiteDbModels;
using System;
using System.Collections.Generic;

namespace DatabaseGenerator.Models;

public partial class Customer
{
    public long CustomerId { get; set; }

    public string? CustomerName { get; set; }

    public string? ContactName { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? PostalCode { get; set; }

    public string? Country { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    
    public CustomerLDb MapToLDb() {
        return new CustomerLDb() {
            CustomerId = this.CustomerId,
            CustomerName = this.CustomerName,
            ContactName = this.ContactName,
            Address = this.Address,
            City = this.City,
            PostalCode = this.PostalCode,
            Country = this.Country
        };
    }
}
