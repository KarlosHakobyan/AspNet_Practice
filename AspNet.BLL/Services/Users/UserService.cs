
using AspNet.BLL.Common.Results;
using AspNet.Data.Identity;
using AspNet.Domain.Models.Users;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AspNet.BLL.Services.Users;

public class UserService(
    UserManager<ApplicationUser> userManager) : IUserService
{
    public async Task<Result<IReadOnlyList<UserListModel>>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var users = await userManager.Users
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var result = new List<UserListModel>();

        foreach (var user in users)
        {
            var roles = await userManager.GetRolesAsync(user);

            result.Add(new UserListModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email ?? string.Empty,
                Roles = roles
            });
        }

        return Result<IReadOnlyList<UserListModel>>.Success(result);
    }

    public async Task<Result<UserDetailsModel>> GetByIdAsync(
        string userId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Result<UserDetailsModel>.Failure(
                new Error(
                    "Users.InvalidId",
                    "User ID cannot be empty.",
                    ErrorType.Validation));
        }

        var user = await userManager.FindByIdAsync(userId);

        if (user is null)
        {
            return Result<UserDetailsModel>.Failure(
                new Error(
                    "Users.NotFound",
                    "User was not found.",
                    ErrorType.NotFound));
        }

        var roles = await userManager.GetRolesAsync(user);
        var isLockedOut = await userManager.IsLockedOutAsync(user);

        return Result<UserDetailsModel>.Success(
            new UserDetailsModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email ?? string.Empty,
                PhoneNumber = user.PhoneNumber,
                Roles = roles,
                EmailConfirmed = user.EmailConfirmed,
                IsLockedOut = isLockedOut,
                LockoutEnd = user.LockoutEnd
            });
    }

    public async Task<Result<UserDetailsModel>> UpdateAsync(
        string userId,
        UserUpdateModel model,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Result<UserDetailsModel>.Failure(
                new Error(
                    "Users.InvalidId",
                    "User ID cannot be empty.",
                    ErrorType.Validation));
        }

        if (model is null)
        {
            return Result<UserDetailsModel>.Failure(
                new Error(
                    "Users.InvalidModel",
                    "User update model cannot be null.",
                    ErrorType.Validation));
        }

        var user = await userManager.FindByIdAsync(userId);

        if (user is null)
        {
            return Result<UserDetailsModel>.Failure(
                new Error(
                    "Users.NotFound",
                    "User was not found.",
                    ErrorType.NotFound));
        }

        user.FirstName = model.FirstName?.Trim();
        user.LastName = model.LastName?.Trim();
        user.PhoneNumber = model.PhoneNumber?.Trim();

        var updateResult = await userManager.UpdateAsync(user);

        if (!updateResult.Succeeded)
        {
            var errors = string.Join(
                "; ",
                updateResult.Errors.Select(error => error.Description));

            return Result<UserDetailsModel>.Failure(
                new Error(
                    "Users.UpdateFailed",
                    errors,
                    ErrorType.Validation));
        }

        return await GetByIdAsync(userId, cancellationToken);
    }

    public async Task<Result<UserDetailsModel>> UpdateRolesAsync(
        string userId,
        UserRolesUpdateModel model,
        string currentAdminId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(userId) ||
            string.IsNullOrWhiteSpace(currentAdminId))
        {
            return Result<UserDetailsModel>.Failure(
                new Error(
                    "Users.InvalidId",
                    "User ID cannot be empty.",
                    ErrorType.Validation));
        }

        if (model is null || model.Roles is null)
        {
            return Result<UserDetailsModel>.Failure(
                new Error(
                    "Users.InvalidModel",
                    "Roles cannot be null.",
                    ErrorType.Validation));
        }

        var user = await userManager.FindByIdAsync(userId);

        if (user is null)
        {
            return Result<UserDetailsModel>.Failure(
                new Error(
                    "Users.NotFound",
                    "User was not found.",
                    ErrorType.NotFound));
        }

        var requestedRoles = model.Roles
            .Where(role => !string.IsNullOrWhiteSpace(role))
            .Select(role => role.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        var allowedRoles = new[]
        {
            RoleNames.Customer,
            RoleNames.Manager,
            RoleNames.Admin
        };

        if (requestedRoles.Any(role =>
            !allowedRoles.Contains(
                role,
                StringComparer.OrdinalIgnoreCase)))
        {
            return Result<UserDetailsModel>.Failure(
                new Error(
                    "Users.InvalidRole",
                    "One or more requested roles are invalid.",
                    ErrorType.Validation));
        }

        var normalizedRoles = requestedRoles
            .Select(role => allowedRoles.First(
                allowed => string.Equals(
                    allowed,
                    role,
                    StringComparison.OrdinalIgnoreCase)))
            .ToList();

        var currentRoles = await userManager.GetRolesAsync(user);

        var isCurrentAdmin = string.Equals(
            userId,
            currentAdminId,
            StringComparison.Ordinal);

        var isTargetAdmin = currentRoles.Contains(
            RoleNames.Admin,
            StringComparer.OrdinalIgnoreCase);

        var isRemovingAdminRole = !normalizedRoles.Contains(
            RoleNames.Admin,
            StringComparer.OrdinalIgnoreCase);

        // Admin cannot remove their own Admin role.
        if (isCurrentAdmin && isTargetAdmin && isRemovingAdminRole)
        {
            return Result<UserDetailsModel>.Failure(
                new Error(
                    "Users.CannotRemoveOwnAdminRole",
                    "You cannot remove your own Admin role.",
                    ErrorType.Forbidden));
        }

        // Protect the last administrator.
        if (isTargetAdmin && isRemovingAdminRole)
        {
            var admins = await userManager.GetUsersInRoleAsync(
                RoleNames.Admin);

            if (admins.Count <= 1)
            {
                return Result<UserDetailsModel>.Failure(
                    new Error(
                        "Users.LastAdmin",
                        "Cannot remove the Admin role from the last administrator.",
                        ErrorType.Conflict));
            }
        }

        var rolesToRemove = currentRoles
            .Where(role => !normalizedRoles.Contains(
                role,
                StringComparer.OrdinalIgnoreCase))
            .ToList();

        var rolesToAdd = normalizedRoles
            .Where(role => !currentRoles.Contains(
                role,
                StringComparer.OrdinalIgnoreCase))
            .ToList();

        if (rolesToRemove.Count > 0)
        {
            var removeResult = await userManager.RemoveFromRolesAsync(
                user,
                rolesToRemove);

            if (!removeResult.Succeeded)
            {
                return Result<UserDetailsModel>.Failure(
                    new Error(
                        "Users.RemoveRolesFailed",
                        string.Join(
                            "; ",
                            removeResult.Errors.Select(
                                error => error.Description)),
                        ErrorType.Failure));
            }
        }

        if (rolesToAdd.Count > 0)
        {
            var addResult = await userManager.AddToRolesAsync(
                user,
                rolesToAdd);

            if (!addResult.Succeeded)
            {
                return Result<UserDetailsModel>.Failure(
                    new Error(
                        "Users.AddRolesFailed",
                        string.Join(
                            "; ",
                            addResult.Errors.Select(
                                error => error.Description)),
                        ErrorType.Failure));
            }
        }

        return await GetByIdAsync(userId, cancellationToken);
    }

    public async Task<Result<UserDetailsModel>> LockAsync(
        string userId,
        UserLockModel model,
        string currentAdminId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(userId) ||
            string.IsNullOrWhiteSpace(currentAdminId))
        {
            return Result<UserDetailsModel>.Failure(
                new Error(
                    "Users.InvalidId",
                    "User ID cannot be empty.",
                    ErrorType.Validation));
        }

        if (model is null ||
            model.LockoutDays <= 0 ||
            model.LockoutDays > 365)
        {
            return Result<UserDetailsModel>.Failure(
                new Error(
                    "Users.InvalidLockoutDays",
                    "Lockout days must be between 1 and 365.",
                    ErrorType.Validation));
        }

        // Admin cannot lock their own account.
        if (string.Equals(
            userId,
            currentAdminId,
            StringComparison.Ordinal))
        {
            return Result<UserDetailsModel>.Failure(
                new Error(
                    "Users.CannotLockOwnAccount",
                    "You cannot lock your own account.",
                    ErrorType.Forbidden));
        }

        var user = await userManager.FindByIdAsync(userId);

        if (user is null)
        {
            return Result<UserDetailsModel>.Failure(
                new Error(
                    "Users.NotFound",
                    "User was not found.",
                    ErrorType.NotFound));
        }

        var roles = await userManager.GetRolesAsync(user);

        // Protect the last available administrator.
        if (roles.Contains(
            RoleNames.Admin,
            StringComparer.OrdinalIgnoreCase))
        {
            var admins = await userManager.GetUsersInRoleAsync(
                RoleNames.Admin);

            var availableAdmins = 0;

            foreach (var admin in admins)
            {
                if (!await userManager.IsLockedOutAsync(admin))
                {
                    availableAdmins++;
                }
            }

            if (availableAdmins <= 1 &&
                !await userManager.IsLockedOutAsync(user))
            {
                return Result<UserDetailsModel>.Failure(
                    new Error(
                        "Users.LastAvailableAdmin",
                        "Cannot lock the last available administrator.",
                        ErrorType.Conflict));
            }
        }

        var enableLockoutResult =
            await userManager.SetLockoutEnabledAsync(user, true);

        if (!enableLockoutResult.Succeeded)
        {
            return Result<UserDetailsModel>.Failure(
                new Error(
                    "Users.EnableLockoutFailed",
                    string.Join(
                        "; ",
                        enableLockoutResult.Errors.Select(
                            error => error.Description)),
                    ErrorType.Failure));
        }

        var lockoutEnd = DateTimeOffset.UtcNow.AddDays(
            model.LockoutDays);

        var lockResult = await userManager.SetLockoutEndDateAsync(
            user,
            lockoutEnd);

        if (!lockResult.Succeeded)
        {
            return Result<UserDetailsModel>.Failure(
                new Error(
                    "Users.LockFailed",
                    string.Join(
                        "; ",
                        lockResult.Errors.Select(
                            error => error.Description)),
                    ErrorType.Failure));
        }

        return await GetByIdAsync(userId, cancellationToken);
    }

    public async Task<Result<UserDetailsModel>> UnlockAsync(
        string userId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Result<UserDetailsModel>.Failure(
                new Error(
                    "Users.InvalidId",
                    "User ID cannot be empty.",
                    ErrorType.Validation));
        }

        var user = await userManager.FindByIdAsync(userId);

        if (user is null)
        {
            return Result<UserDetailsModel>.Failure(
                new Error(
                    "Users.NotFound",
                    "User was not found.",
                    ErrorType.NotFound));
        }

        var unlockResult = await userManager.SetLockoutEndDateAsync(
            user,
            null);

        if (!unlockResult.Succeeded)
        {
            return Result<UserDetailsModel>.Failure(
                new Error(
                    "Users.UnlockFailed",
                    string.Join(
                        "; ",
                        unlockResult.Errors.Select(
                            error => error.Description)),
                    ErrorType.Failure));
        }

        return await GetByIdAsync(userId, cancellationToken);
    }
}
