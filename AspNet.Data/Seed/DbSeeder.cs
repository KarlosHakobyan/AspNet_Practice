using AspNet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AspNet.Data.Seed
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var context = scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

            // 1. Categories

            var categoryNames = new[]
            {
                "Electronics",
                "Computers",
                "Smartphones",
                "Accessories"
            };

            foreach (var name in categoryNames)
            {
                if (!await context.Categories.AnyAsync(c => c.Name == name))
                {
                    context.Categories.Add(new Category
                    {
                        Name = name
                    });
                }
            }

            await context.SaveChangesAsync();

            var electronics = await context.Categories
                .FirstAsync(c => c.Name == "Electronics");

            var computers = await context.Categories
                .FirstAsync(c => c.Name == "Computers");

            var smartphones = await context.Categories
                .FirstAsync(c => c.Name == "Smartphones");

            // 2. Products

            var productData = new[]
            {
                new { Name = "Dell XPS 15", Price = 1500m, CategoryId = computers.Id },
                new { Name = "Samsung Galaxy S25", Price = 1000m, CategoryId = smartphones.Id },
                new { Name = "iPhone 17 Pro", Price = 1200m, CategoryId = smartphones.Id },
                new { Name = "Logitech MX Master", Price = 100m, CategoryId = electronics.Id }
            };

            foreach (var item in productData)
            {
                if (!await context.Products.AnyAsync(p => p.Name == item.Name))
                {
                    context.Products.Add(new Product
                    {
                        Name = item.Name,
                        Price = item.Price,
                        CategoryId = item.CategoryId
                    });
                }
            }

            await context.SaveChangesAsync();

            // 3. ProductDetails (One-to-One)

            var products = await context.Products.ToListAsync();

            foreach (var product in products)
            {
                if (!await context.ProductDetails
                    .AnyAsync(d => d.ProductId == product.Id))
                {
                    context.ProductDetails.Add(new ProductDetails
                    {
                        ProductId = product.Id,
                        Description = $"{product.Name} - Product description",
                        Manufacturer = product.Name.Split(' ')[0],
                        WarrantyMonths = 24
                    });
                }
            }

            await context.SaveChangesAsync();

            // 4. Tags

            var tagNames = new[]
            {
                "Gaming",
                "Premium",
                "Portable",
                "Wireless",
                "Popular"
            };

            foreach (var name in tagNames)
            {
                if (!await context.Tags.AnyAsync(t => t.Name == name))
                {
                    context.Tags.Add(new Tag
                    {
                        Name = name
                    });
                }
            }

            await context.SaveChangesAsync();

            // 5. ProductTags (Many-to-Many)

            var tags = await context.Tags.ToListAsync();

            var tagMap = tags.ToDictionary(t => t.Name);

            var productMap = products.ToDictionary(p => p.Name);

            var productTagData = new[]
            {
                ("Dell XPS 15", "Gaming"),
                ("Dell XPS 15", "Premium"),
                ("Dell XPS 15", "Portable"),
                ("Samsung Galaxy S25", "Premium"),
                ("Samsung Galaxy S25", "Popular"),
                ("iPhone 17 Pro", "Premium"),
                ("iPhone 17 Pro", "Popular"),
                ("Logitech MX Master", "Wireless")
            };

            foreach (var (productName, tagName) in productTagData)
            {
                var product = productMap[productName];
                var tag = tagMap[tagName];

                bool exists = await context.ProductTags.AnyAsync(pt =>
                    pt.ProductId == product.Id &&
                    pt.TagId == tag.Id);

                if (!exists)
                {
                    context.ProductTags.Add(new ProductTag
                    {
                        ProductId = product.Id,
                        TagId = tag.Id,
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }

            await context.SaveChangesAsync();
        }
    }
}