using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<CategoryProduct> CategoryProducts { get; set; } = new List<CategoryProduct>();
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}