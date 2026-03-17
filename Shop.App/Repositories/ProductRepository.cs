using Microsoft.EntityFrameworkCore;
using Shop.App.Data;
using Shop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.App.Repositories;

public class ProductRepository
{
    private readonly ShopDbContext _context;

    public ProductRepository(ShopDbContext context)
    {
        _context = context;
    }

    public void Add(Product product)
    {
        _context.Products.Add(product);
        _context.SaveChanges();
    }

    public List<Product> GetAll()
    {
        return _context.Products.AsNoTracking().ToList();
    }

    public void UpdatePrice(int productId, decimal newPrice)
    {
        var product = _context.Products.FirstOrDefault(p => p.Id == productId);
        if (product == null)
        {
            Console.WriteLine("Продукт не знайдено.");
            return;
        }

        product.Price = newPrice;
        _context.SaveChanges();
    }

    public void Delete(int productId)
    {
        var product = _context.Products.FirstOrDefault(p => p.Id == productId);
        if (product == null)
        {
            Console.WriteLine("Продукт не знайдено.");
            return;
        }

        _context.Products.Remove(product);
        _context.SaveChanges();
    }
}