using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shop.App.Configurators;
using Shop.App.Data;
using Shop.App.Repositories;
using Shop.App.Seeders;
using Shop.App.Services;
using Shop.Domain.Entities;
using Shop.Domain.Enums;

namespace Shop.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection(); //Створюється контейнер DI (Dependency Injection)

            services.AddDbContext<ShopDbContext>(options =>
            {
                DbContextConfigurator.Configure(options);
            });

            services.AddScoped<OrderService>();
            services.AddScoped<ProductRepository>();
            services.AddScoped<ProductService>();
            services.AddScoped<ShopManager>();

            var provider = services.BuildServiceProvider();

            //"Один DbContext на одну операцію роботи з БД"

            using var scope = provider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ShopDbContext>();

            if (context.Database.CanConnect())
            {
                var connectionString = context.Database.GetConnectionString();

                
                var seeder = new ProductSeeder(connectionString!);
                seeder.Seed(100000);

                var benchmarkService = new ProductBenchmarkService(connectionString!);

                
                benchmarkService.SearchWithoutIndex("Product_50000");

                
                benchmarkService.CreateIndex();

                
                benchmarkService.SearchWithIndex("Product_50000");
            }
            else
            {
                Console.WriteLine("Не вдалось підключитись до БД");
            }
        }
    }
}
