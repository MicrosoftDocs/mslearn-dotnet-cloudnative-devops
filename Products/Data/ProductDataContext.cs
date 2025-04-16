using DataEntities;
using Microsoft.EntityFrameworkCore;

namespace Products.Data;

public sealed class ProductDataContext(DbContextOptions<ProductDataContext> options)
    : DbContext(options)
{
    public DbSet<Product> Product { get; set; } = default!;
}

internal static class Extensions
{
    public static async Task CreateDbIfNotExistsAsync(this IHost host)
    {
        using var scope = host.Services.CreateScope();

        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<ProductDataContext>();

        await context.Database.EnsureCreatedAsync();

        await DbInitializer.InitializeAsync(context);
    }
}

internal static class DbInitializer
{
    public static async Task InitializeAsync(ProductDataContext context)
    {
        if (context.Product.Any())
        {
            return;
        }

        List<Product> products =
        [
            new() { Name = "Solar Powered Flashlight", Description = "A fantastic product for outdoor enthusiasts", Price = 19.99m, ImageUrl = "/api/product/product1.png" },
            new() { Name = "Hiking Poles", Description = "Ideal for camping and hiking trips", Price = 24.99m, ImageUrl = "/api/product/product2.png" },
            new() { Name = "Outdoor Rain Jacket", Description = "This product will keep you warm and dry in all weathers", Price = 49.99m, ImageUrl = "/api/product/product3.png" },
            new() { Name = "Survival Kit", Description = "A must-have for any outdoor adventurer", Price = 99.99m, ImageUrl = "/api/product/product4.png" },
            new() { Name = "Outdoor Backpack", Description = "This backpack is perfect for carrying all your outdoor essentials", Price = 39.99m, ImageUrl = "/api/product/product5.png" },
            new() { Name = "Camping Cookware", Description = "This cookware set is ideal for cooking outdoors", Price = 29.99m, ImageUrl = "/api/product/product6.png" },
            new() { Name = "Camping Stove", Description = "This stove is perfect for cooking outdoors", Price = 49.99m, ImageUrl = "/api/product/product7.png" },
            new() { Name = "Camping Lantern", Description = "This lantern is perfect for lighting up your campsite", Price = 19.99m, ImageUrl = "/api/product/product8.png" },
            new() { Name = "Camping Tent", Description = "This tent is perfect for camping trips", Price = 99.99m, ImageUrl = "/api/product/product9.png" },
        ];

        context.AddRange(products);

        await context.SaveChangesAsync();
    }
}
