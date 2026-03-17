using Shop.App.Repositories;
using Shop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.App.Services;

public class ProductService
{
    private readonly ProductRepository _repository;

    public ProductService(ProductRepository repository)
    {
        _repository = repository;
    }

    public void AddProduct(string name, decimal price, int stockQuantity)
    {
        var product = new Product
        {
            Name = name,
            Price = price,
            StockQuantity = stockQuantity,
            CreatedAt = DateTime.Now
        };

        _repository.Add(product);
        Console.WriteLine($"Продукт '{name}' успішно додано.");
    }

    public List<Product> GetAllProducts()
    {
        return _repository.GetAll();
    }

    public void UpdateProductPrice(int productId, decimal newPrice)
    {
        _repository.UpdatePrice(productId, newPrice);
        Console.WriteLine($"Ціну продукту #{productId} оновлено до {newPrice} грн.");
    }

    public void DeleteProduct(int productId)
    {
        _repository.Delete(productId);
        Console.WriteLine($"Продукт #{productId} видалено.");
    }
}