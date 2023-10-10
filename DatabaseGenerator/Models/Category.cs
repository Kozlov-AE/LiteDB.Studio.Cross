using DatabaseGenerator.Models.LiteDbModels;
using System;
using System.Collections.Generic;

namespace DatabaseGenerator.Models;

public partial class Category
{
    public long CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public CategoryLDb MapToLDb() {
        return new CategoryLDb() {
            CategoryId = this.CategoryId,
            CategoryName = this.CategoryName,
            Description = this.Description
        };
    }
}
