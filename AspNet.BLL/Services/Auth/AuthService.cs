using AspNet.Api.Services.Auth;
using AspNet.BLL.Common.Results;
using AspNet.Data.Identity;
using AspNet.Domain.Models.AuthModels;

using Microsoft.AspNetCore.Identity;

namespace AspNet.BLL.Services.Auth;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        IJwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<Result> RegisterAsync(RegisterModel model)
    {
        var existingUser = await _userManager.FindByEmailAsync(model.Email);

        if (existingUser is not null)
        {
            return Result.Failure(
                new Error(
                    "Auth.EmailAlreadyExists",
                    "A user with this email already exists.",
                    ErrorType.Conflict
                )
            );
        }

        var user = new ApplicationUser
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            UserName = model.Email
        };

        var createResult = await _userManager.CreateAsync(
            user,
            model.Password
        );

        if (!createResult.Succeeded)
        {
            return Result.Failure(
                new Error(
                    "Auth.RegistrationFailed",
                    string.Join("; ", createResult.Errors.Select(x => x.Description)),
                    ErrorType.Validation
                )
            );
        }

        var roleResult = await _userManager.AddToRoleAsync(
            user,
            RoleNames.Customer
        );

        if (!roleResult.Succeeded)
        {
            await _userManager.DeleteAsync(user);

            return Result.Failure(
                new Error(
                    "Auth.RoleAssignmentFailed",
                    "Unable to assign the default role.",
                    ErrorType.Failure
                )
            );
        }

        return Result.Success();
    }

    public async Task<Result<AuthResponseModel>> LoginAsync(LoginModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user is null)
            return InvalidCredentials();

        if (await _userManager.IsLockedOutAsync(user))
            return InvalidCredentials();

        var passwordValid = await _userManager.CheckPasswordAsync(
            user,
            model.Password
        );

        if (!passwordValid)
            return InvalidCredentials();

        var roles = await _userManager.GetRolesAsync(user);

        if (roles.Count == 0)
        {
            return Result<AuthResponseModel>.Failure(
                new Error(
                    "Auth.UserHasNoRoles",
                    "User does not have any assigned roles.",
                    ErrorType.Forbidden
                )
            );
        }

        var token = await _jwtTokenService.GenerateTokenAsync(user);

        return Result<AuthResponseModel>.Success(
            new AuthResponseModel
            {
                UserId = user.Id,
                Email = user.Email!,
                Roles = roles.ToList(),
                Token = token
            }
        );
    }

    private static Result<AuthResponseModel> InvalidCredentials()
    {
        return Result<AuthResponseModel>.Failure(
            new Error(
                "Auth.InvalidCredentials",
                "Invalid email or password.",
                ErrorType.Unauthorized
            )
        );
    }
}