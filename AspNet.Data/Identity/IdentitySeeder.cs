using AspNet.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace AspNet.Data.Seed;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string[] roles =
        [
            RoleNames.Customer,
            RoleNames.Manager,
            RoleNames.Admin
        ];

        foreach (var role in roles)
        {
            var roleExists = await roleManager.RoleExistsAsync(role);

            if (!roleExists)
            {
                var result = await roleManager.CreateAsync(new IdentityRole(role));

                if (!result.Succeeded)
                {
                    var errors = string.Join("; ",result.Errors.Select(e => e.Description));

                    throw new InvalidOperationException($"Failed to create role '{role}': {errors}");
                }
            }
        }
    }
}