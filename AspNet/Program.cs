
using System.Security.Claims;
using System.Text;

using AspNet.Api.Exceptions;
using AspNet.Api.Middlewares;
using AspNet.Api.Services.Auth;
using AspNet.Api.Settings;

using AspNet.BLL.Mapping;
using AspNet.BLL.Services.Auth;
using AspNet.BLL.Services.Products;
using AspNet.BLL.Validators.Products;

using AspNet.Data;
using AspNet.Data.Identity;
using AspNet.Data.Repositories;
using AspNet.Data.Repositories.Interfaces;
using AspNet.Data.Repositories.UnitOfWork;
using AspNet.Data.Seed;

using FluentValidation;
using FluentValidation.AspNetCore;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace AspNet.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Controllers
        builder.Services.AddControllers()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetails = new ValidationProblemDetails(
                        context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "Validation Failed",
                        Detail = "One or more validation errors occurred.",
                        Instance = context.HttpContext.Request.Path
                    };

                    problemDetails.Extensions["traceId"] =
                        context.HttpContext.TraceIdentifier;

                    return new BadRequestObjectResult(problemDetails);
                };
            });

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

        // Product Services
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<ITagService, TagService>();
        builder.Services.AddScoped<IProductDetailsService, ProductDetailsService>();

        // Identity
        builder.Services
            .AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;

                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        // JWT Settings
        var jwtSettings = builder.Configuration
            .GetSection("JwtSettings")
            .Get<JwtSettings>()
            ?? throw new InvalidOperationException(
                "JWT settings are missing."
            );

        if (string.IsNullOrWhiteSpace(jwtSettings.Issuer) ||
            string.IsNullOrWhiteSpace(jwtSettings.Audience) ||
            string.IsNullOrWhiteSpace(jwtSettings.SecretKey))
        {
            throw new InvalidOperationException(
                "JWT configuration is incomplete."
            );
        }

        if (Encoding.UTF8.GetByteCount(jwtSettings.SecretKey) < 32)
        {
            throw new InvalidOperationException(
                "JWT SecretKey must be at least 32 bytes for HS256."
            );
        }

        if (jwtSettings.ExpirationInMinutes <= 0)
        {
            throw new InvalidOperationException(
                "JWT ExpirationInMinutes must be greater than zero."
            );
        }

        builder.Services.Configure<JwtSettings>(
            builder.Configuration.GetSection("JwtSettings")
        );

        // Authentication Services
        builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
        builder.Services.AddScoped<IAuthService, AuthService>();

        // JWT Authentication
        builder.Services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                    JwtBearerDefaults.AuthenticationScheme;

                options.DefaultChallengeScheme =
                    JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtSettings.Issuer,

                        ValidateAudience = true,
                        ValidAudience = jwtSettings.Audience,

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(1),

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSettings.SecretKey)
                        ),

                        RoleClaimType = ClaimTypes.Role,
                        NameClaimType = ClaimTypes.Name
                    };
            });

        // Authorization
        builder.Services.AddAuthorization();

        // AutoMapper
        builder.Services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(typeof(ProductProfile).Assembly);
        });

        // FluentValidation
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddValidatorsFromAssemblyContaining<ProductAddModelValidator>();

        // Global Exception Handling
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();

        // Custom Middleware
        builder.Services.AddTransient<RequestLoggingMiddleware>();

        // Swagger
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "AspNet API",
                Version = "v1",
                Description = "ASP.NET Core Web API with Identity and JWT Authentication"
            });

            options.AddSecurityDefinition("Bearer",
                new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your JWT token."
                });

            options.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
        });

        var app = builder.Build();

        // Exception Handling
        app.UseExceptionHandler();

        // Request Logging
        app.UseMiddleware<RequestLoggingMiddleware>();

        // Swagger
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Authentication & Authorization
        app.UseAuthentication();
        app.UseAuthorization();

        // Controllers
        app.MapControllers();

        // Database Seeding
        try
        {
            await DbSeeder.SeedAsync(app.Services);
            await IdentitySeeder.SeedAsync(app.Services);
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
