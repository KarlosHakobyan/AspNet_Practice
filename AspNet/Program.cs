using AspNet.Data;
using AspNet.Data.Seed;
using AspNet.Repositories;
using AspNet.Repositories.Interfaces;
using AspNet.Repositories.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace AspNet
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                )
            );

            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ITagRepository, TagRepository>();
            builder.Services.AddScoped<IProductDetailsRepository, ProductDetailsRepository>();
            builder.Services.AddScoped<IProductTagRepository, ProductTagRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();

            // Database seeding
            try
            {
                await DbSeeder.SeedAsync(app.Services);
            }
            catch (Exception ex)
            {
                Console.WriteLine("SEEDING ERROR:");
                Console.WriteLine(ex.ToString());
                throw;
            }

            app.Run();
        }
    }
}