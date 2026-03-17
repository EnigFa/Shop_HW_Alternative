using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.App.Seeders;

public class ProductSeeder
{
    private readonly string _connectionString;

    public ProductSeeder(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void Seed(int count = 100000)
    {
        using var connection = new SqlConnection(_connectionString);

        Console.WriteLine($"Починаємо вставку {count} продуктів...");

        var products = new List<object>();
        for (int i = 1; i <= count; i++)
        {
            products.Add(new
            {
                Name = $"Product_{i}",
                Price = Math.Round(new Random().NextDouble() * 10000, 2),
                StockQuantity = new Random().Next(1, 500),
                CreatedAt = DateTime.Now
            });
        }

        connection.Execute(
            "INSERT INTO Products (Name, Price, StockQuantity, CreatedAt) VALUES (@Name, @Price, @StockQuantity, @CreatedAt)",
            products
        );

        Console.WriteLine($"Вставку завершено. Додано {count} продуктів.");
    }
}