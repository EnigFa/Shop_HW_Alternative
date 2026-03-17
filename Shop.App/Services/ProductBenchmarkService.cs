using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.App.Services;

public class ProductBenchmarkService
{
    private readonly string _connectionString;

    public ProductBenchmarkService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void SearchWithoutIndex(string productName)
    {
        using var connection = new SqlConnection(_connectionString);

        var stopwatch = Stopwatch.StartNew();

        var result = connection.Query<string>(
            "SELECT Name FROM Products WHERE Name = @Name",
            new { Name = productName }
        ).ToList();

        stopwatch.Stop();

        Console.WriteLine($"\nПошук БЕЗ індексу:");
        Console.WriteLine($"Знайдено записів: {result.Count}");
        Console.WriteLine($"Запит виконався за {stopwatch.ElapsedMilliseconds} ms");
    }

    public void CreateIndex()
    {
        using var connection = new SqlConnection(_connectionString);

        connection.Execute("CREATE INDEX IX_Products_Name ON Products (Name)");
        Console.WriteLine("\nІндекс IX_Products_Name успішно створено.");
    }

    public void SearchWithIndex(string productName)
    {
        using var connection = new SqlConnection(_connectionString);

        var stopwatch = Stopwatch.StartNew();

        var result = connection.Query<string>(
            "SELECT Name FROM Products WHERE Name = @Name",
            new { Name = productName }
        ).ToList();

        stopwatch.Stop();

        Console.WriteLine($"\nПошук З індексом:");
        Console.WriteLine($"Знайдено записів: {result.Count}");
        Console.WriteLine($"Запит виконався за {stopwatch.ElapsedMilliseconds} ms");

        
        // Тест проводився на таблиці з 100 000 записів.
       
        // Результат: пошук з індексом виявився критично швидшим порівняно з пошуком без індексу (97ms проти 1ms).
    }
}