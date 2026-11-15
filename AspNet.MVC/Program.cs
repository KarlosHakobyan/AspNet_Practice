
using AspNet.MVC.Services.Api;
using AspNet.MVC.Services.Users;

using Microsoft.AspNetCore.Authentication.Cookies;

namespace AspNet.MVC;

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

        if (!Uri.TryCreate(
                apiBaseUrl,
                UriKind.Absolute,
                out var apiUri))
        {
            throw new InvalidOperationException(
                "ApiSettings:BaseUrl must be a valid absolute URL.");
        }

        // HttpContext Accessor
        builder.Services.AddHttpContextAccessor();

        // JWT Authorization Handler
        builder.Services.AddTransient<JwtAuthorizationHandler>();

        // Named HTTP Client for AccountController
        builder.Services.AddHttpClient("Api", client =>
        {
            client.BaseAddress = apiUri;
        });

        // Products API
        builder.Services.AddHttpClient<
            IProductApiService,
            ProductApiService>(client =>
            {
                client.BaseAddress = apiUri;
            })
        .AddHttpMessageHandler<JwtAuthorizationHandler>();

        // Categories API
        builder.Services.AddHttpClient<
            ICategoryApiService,
            CategoryApiService>(client =>
            {
                client.BaseAddress = apiUri;
            })
        .AddHttpMessageHandler<JwtAuthorizationHandler>();

        // Tags API
        builder.Services.AddHttpClient<
            ITagApiService,
            TagApiService>(client =>
            {
                client.BaseAddress = apiUri;
            })
        .AddHttpMessageHandler<JwtAuthorizationHandler>();

        // Product Details API
        builder.Services.AddHttpClient<
            IProductDetailsApiService,
            ProductDetailsApiService>(client =>
            {
                client.BaseAddress = apiUri;
            })
        .AddHttpMessageHandler<JwtAuthorizationHandler>();

        // Users API
        builder.Services.AddHttpClient<
            IUserApiService,
            UserApiService>(client =>
            {
                client.BaseAddress = apiUri;
            })
        .AddHttpMessageHandler<JwtAuthorizationHandler>();

        // Cookie Authentication
        builder.Services
            .AddAuthentication(
                CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";

                options.Cookie.Name = "AspNet.MVC.Auth";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy =
                    CookieSecurePolicy.Always;

                options.Cookie.SameSite = SameSiteMode.Lax;

                options.ExpireTimeSpan = TimeSpan.FromHours(8);
                options.SlidingExpiration = true;
            });

        // Authorization
        builder.Services.AddAuthorization();

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

        // Authentication must come before Authorization
        app.UseAuthentication();
        app.UseAuthorization();

        // Routing
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Account}/{action=Login}/{id?}");

        app.Run();
    }
}
