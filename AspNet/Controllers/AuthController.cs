
using AspNet.Api.Extensions;
using AspNet.BLL.Services.Auth;
using AspNet.Domain.Models.AuthModels;

using Microsoft.AspNetCore.Mvc;

namespace AspNet.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterModel model)
    {
        var result = await _authService.RegisterAsync(model);

        if (result.IsFailure)
        {
            return this.ToActionResult(result);
        }

        return StatusCode(
            StatusCodes.Status201Created,
            new
            {
                message = "User registered successfully."
            }
        );
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginModel model)
    {
        var result = await _authService.LoginAsync(model);

        return this.ToActionResult(result);
    }
}
