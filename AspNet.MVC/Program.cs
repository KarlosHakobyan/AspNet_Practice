using AspNet.MVC.Services.Api;

namespace AspNet.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // MVC
            builder.Services.AddControllersWithViews();

            // API Configuration
            var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"]
                ?? throw new InvalidOperationException(
                    "ApiSettings:BaseUrl is not configured.");

            // HTTP Client
            builder.Services.AddHttpClient<IProductApiService, ProductApiService>(
                client =>
                {
                    client.BaseAddress = new Uri(apiBaseUrl);
                });

            var app = builder.Build();

            // Middleware
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}