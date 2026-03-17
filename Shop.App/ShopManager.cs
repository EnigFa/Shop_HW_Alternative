using Shop.App.Services;
using Shop.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.App;

public class ShopManager
{
    private readonly ProductService _productService;

    public ShopManager(ProductService productService)
    {
        _productService = productService;
    }

    public void Run()
    {
        while (true)
        {
            ShowMenu();

            Console.Write("Ваш вибір: ");
            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Невірний ввід.");
                continue;
            }

            var action = (ProductMenuAction)choice;

            switch (action)
            {
                case ProductMenuAction.AddProduct:
                    AddProduct();
                    break;
                case ProductMenuAction.GetAllProducts:
                    GetAllProducts();
                    break;
                case ProductMenuAction.UpdatePrice:
                    UpdatePrice();
                    break;
                case ProductMenuAction.DeleteProduct:
                    DeleteProduct();
                    break;
                case ProductMenuAction.Exit:
                    Console.WriteLine("Вихід з програми.");
                    return;
                default:
                    Console.WriteLine("Невідома команда.");
                    break;
            }
        }
    }

    private void ShowMenu()
    {
        Console.WriteLine("\n--- Меню управління продуктами ---");
        Console.WriteLine($"{(int)ProductMenuAction.AddProduct} - Додати продукт");
        Console.WriteLine($"{(int)ProductMenuAction.GetAllProducts} - Показати всі продукти");
        Console.WriteLine($"{(int)ProductMenuAction.UpdatePrice} - Оновити ціну продукту");
        Console.WriteLine($"{(int)ProductMenuAction.DeleteProduct} - Видалити продукт");
        Console.WriteLine($"{(int)ProductMenuAction.Exit} - Вийти");
    }

    private void AddProduct()
    {
        Console.Write("Введіть назву продукту: ");
        var name = Console.ReadLine() ?? string.Empty;

        Console.Write("Введіть ціну: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal price))
        {
            Console.WriteLine("Невірна ціна.");
            return;
        }

        Console.Write("Введіть кількість на складі: ");
        if (!int.TryParse(Console.ReadLine(), out int stockQuantity))
        {
            Console.WriteLine("Невірна кількість.");
            return;
        }

        _productService.AddProduct(name, price, stockQuantity);
    }

    private void GetAllProducts()
    {
        var products = _productService.GetAllProducts();

        if (products.Count == 0)
        {
            Console.WriteLine("Продуктів не знайдено.");
            return;
        }

        Console.WriteLine("\n--- Список продуктів ---");
        foreach (var p in products)
        {
            Console.WriteLine($"  [{p.Id}] {p.Name} - {p.Price} грн (на складі: {p.StockQuantity})");
        }
    }

    private void UpdatePrice()
    {
        Console.Write("Введіть Id продукту: ");
        if (!int.TryParse(Console.ReadLine(), out int productId))
        {
            Console.WriteLine("Невірний Id.");
            return;
        }

        Console.Write("Введіть нову ціну: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal newPrice))
        {
            Console.WriteLine("Невірна ціна.");
            return;
        }

        _productService.UpdateProductPrice(productId, newPrice);
    }

    private void DeleteProduct()
    {
        Console.Write("Введіть Id продукту: ");
        if (!int.TryParse(Console.ReadLine(), out int productId))
        {
            Console.WriteLine("Невірний Id.");
            return;
        }

        _productService.DeleteProduct(productId);
    }
}