using Microsoft.EntityFrameworkCore;
using Shop.App.Data;
using Shop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.App.Services;

public class OrderService
{
    private readonly ShopDbContext _context;

    public OrderService(ShopDbContext context)
    {
        _context = context;
    }

    public void CreateOrder(int userId)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
        {
            Console.WriteLine("Користувача не знайдено.");
            return;
        }

        var order = new Order
        {
            UserId = userId,
            Status = "Формується",
            OrderDate = DateTime.Now
        };

        var orderItems = new List<OrderItem>();

        while (true)
        {
            Console.WriteLine("\n--- Додавання продукту до замовлення ---");
            Console.WriteLine("Доступні продукти:");

            var products = _context.Products.ToList();
            foreach (var p in products)
            {
                Console.WriteLine($"  [{p.Id}] {p.Name} - {p.Price} грн (на складі: {p.StockQuantity})");
            }

            Console.Write("Введіть Id продукту (або 0 щоб завершити додавання): ");
            if (!int.TryParse(Console.ReadLine(), out int productId) || productId == 0)
                break;

            var product = products.FirstOrDefault(p => p.Id == productId);
            if (product == null)
            {
                Console.WriteLine("Продукт не знайдено.");
                continue;
            }

            Console.Write("Введіть кількість: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
            {
                Console.WriteLine("Невірна кількість.");
                continue;
            }

            var existingItem = orderItems.FirstOrDefault(oi => oi.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                orderItems.Add(new OrderItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    Price = product.Price
                });
            }

            Console.WriteLine($"Продукт '{product.Name}' x{quantity} додано до замовлення.");

            Console.WriteLine("\nПоточне замовлення:");
            foreach (var item in orderItems)
            {
                var itemProduct = products.First(p => p.Id == item.ProductId);
                Console.WriteLine($"  {itemProduct.Name} x{item.Quantity} = {item.Price * item.Quantity} грн");
            }
            Console.WriteLine($"  Разом: {orderItems.Sum(oi => oi.Price * oi.Quantity)} грн");

            Console.WriteLine("\n1 - Додати ще продукт");
            Console.WriteLine("2 - Завершити замовлення");
            Console.WriteLine("3 - Скасувати замовлення");
            Console.Write("Ваш вибір: ");

            var choice = Console.ReadLine();
            if (choice == "2")
                break;
            else if (choice == "3")
            {
                Console.WriteLine("Замовлення скасовано.");
                return;
            }
        }

        if (orderItems.Count == 0)
        {
            Console.WriteLine("Замовлення порожнє. Збереження скасовано.");
            return;
        }

        order.TotalAmount = orderItems.Sum(oi => oi.Price * oi.Quantity);
        order.OrderItems = orderItems;

        _context.Orders.Add(order);
        _context.SaveChanges();

        Console.WriteLine($"\nЗамовлення #{order.Id} успішно збережено! Сума: {order.TotalAmount} грн");
    }
}
