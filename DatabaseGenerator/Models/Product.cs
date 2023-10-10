using DatabaseGenerator.Models.LiteDbModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace DatabaseGenerator.Models;

public partial class Product
{
    public long ProductId { get; set; }

    public string? ProductName { get; set; }

    public long? SupplierId { get; set; }

    public long? CategoryId { get; set; }

    public string? Unit { get; set; }

    public byte[]? Price { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Supplier? Supplier { get; set; }
    
    public ProductLDb MapToLDb() {
        return new ProductLDb() {
            ProductId = this.ProductId,
            ProductName = this.ProductName,
            SupplierId = this.SupplierId,
            CategoryId = this.CategoryId,
            Unit = this.Unit,
            Price = Convert.ToDecimal(Encoding.Default.GetString(this.Price?.Where(x => x != 0).ToArray() ?? Array.Empty<byte>()), CultureInfo.InvariantCulture)
        };
    }
}
