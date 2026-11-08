
using AspNet.Api.Mapping;
using AspNet.Api.Services.Products;
using AspNet.Data;
using AspNet.Data.Repositories;
using AspNet.Data.Repositories.Interfaces;
using AspNet.Data.Repositories.UnitOfWork;
using AspNet.Data.Seed;

using Microsoft.EntityFrameworkCore;

namespace AspNet.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Controllers
            builder.Services.AddControllers();

            // Database
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                )
            );

            // Repositories
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ITagRepository, TagRepository>();
            builder.Services.AddScoped<IProductDetailsRepository, ProductDetailsRepository>();
            builder.Services.AddScoped<IProductTagRepository, ProductTagRepository>();

            // Unit Of Work
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Services
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ITagService, TagService>();
            builder.Services.AddScoped<IProductDetailsService, ProductDetailsService>();

            // AutoMapper
            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(typeof(ProductProfile).Assembly);
            });

            // Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // HTTP Pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();

            // Database Seeding
            try
            {
                await DbSeeder.SeedAsync(app.Services);
            }
            catch (Exception ex)
            {
                Console.WriteLine("SEEDING ERROR:");
                Console.WriteLine(ex);
                throw;
            }

            app.Run();
        }
    }
}
