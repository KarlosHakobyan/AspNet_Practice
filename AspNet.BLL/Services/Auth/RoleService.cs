using AspNet.BLL.Common.Results;
using AspNet.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AspNet.BLL.Services.Auth
{

    public class RoleService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager) : IRoleService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;

        // 1. Get all roles
        public async Task<Result<IReadOnlyList<string>>> GetAllRolesAsync(CancellationToken token)
        {
            var roles = await _roleManager.Roles.Where(role => role.Name != null).Select(role => role.Name!).ToListAsync(token);

            return Result<IReadOnlyList<string>>.Success(roles);
        }

        // 2. Get roles by user ID
        public async Task<Result<IReadOnlyList<string>>> GetUserRolesAsync(string userId, CancellationToken token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                return Result<IReadOnlyList<string>>.Failure(new Error("User.NotFound", "User was not found.", ErrorType.NotFound));
            }

            var roles = await _userManager.GetRolesAsync(user);

            token.ThrowIfCancellationRequested();

            return Result<IReadOnlyList<string>>.Success(roles.ToList());
        }

        // 3. Assign role to user
        public async Task<Result> AssignRoleAsync(string userId, string roleName, CancellationToken token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                return Result.Failure(new Error("User.NotFound", "User was not found.", ErrorType.NotFound));
            }

            var roleExists = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExists)
            {
                return Result.Failure(new Error("Role.NotFound", "Role was not found.", ErrorType.NotFound));
            }

            var isInRole = await _userManager.IsInRoleAsync(user, roleName);

            if (isInRole)
            {
                return Result.Failure(new Error("Role.AlreadyAssigned", "User already has this role.", ErrorType.Conflict));
            }

            token.ThrowIfCancellationRequested();

            var result = await _userManager.AddToRoleAsync(user, roleName);

            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(error => error.Description));

                return Result.Failure(new Error("Role.AssignmentFailed", errors, ErrorType.Failure));
            }

            return Result.Success();
        }

        // 4. Remove role from user

        public async Task<Result> RemoveRoleAsync(
            string userId,
            string roleName,
            CancellationToken token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                return Result.Failure(
                    new Error(
                        "User.NotFound",
                        "User was not found.",
                        ErrorType.NotFound));
            }

            var isInRole = await _userManager.IsInRoleAsync(user, roleName);

            if (!isInRole)
            {
                return Result.Failure(
                    new Error(
                        "Role.NotAssigned",
                        "User does not have this role.",
                        ErrorType.NotFound));
            }

            if (string.Equals(
                roleName,
                RoleNames.Admin,
                StringComparison.OrdinalIgnoreCase))
            {
                var admins = await _userManager.GetUsersInRoleAsync(RoleNames.Admin);

                if (admins.Count <= 1)
                {
                    return Result.Failure(
                        new Error(
                            "Role.LastAdmin",
                            "Cannot remove the role from the last administrator.",
                            ErrorType.Conflict));
                }
            }

            token.ThrowIfCancellationRequested();

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);

            if (!result.Succeeded)
            {
                var errors = string.Join("; ",
                    result.Errors.Select(error => error.Description));

                return Result.Failure(
                    new Error(
                        "Role.RemovalFailed",
                        errors,
                        ErrorType.Failure));
            }

            return Result.Success();
        }

    }
}
