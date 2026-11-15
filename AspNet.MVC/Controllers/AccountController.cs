
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;

using AspNet.MVC.Models.Auth;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNet.MVC.Controllers;

public class AccountController(
    IHttpClientFactory httpClientFactory,
    ILogger<AccountController> logger) : Controller
{
    private readonly HttpClient _api =
        httpClientFactory.CreateClient("Api");

    private readonly ILogger<AccountController> _logger = logger;

    private static readonly HashSet<string> SupportedRoles =
    [
        "Admin",
        "Manager",
        "Customer"
    ];

    // GET: /Account/Register
    [AllowAnonymous]
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    // POST: /Account/Register
    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(
        RegisterViewModel model,
        CancellationToken token)
    {
        if (!ModelState.IsValid)
            return View(model);

        var request = new
        {
            model.FirstName,
            model.LastName,
            model.Email,
            model.Password
        };

        try
        {
            using var response = await _api.PostAsJsonAsync(
                "api/auth/register",
                request,
                token);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Registration failed. Please check your information.");

                return View(model);
            }

            TempData["Success"] =
                "Registration successful. You can now log in.";

            return RedirectToAction(nameof(Login));
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(
                ex,
                "Unable to connect to the API during registration.");

            ModelState.AddModelError(
                string.Empty,
                "Unable to connect to the API. Please try again later.");

            return View(model);
        }
    }

    // GET: /Account/Login
    [AllowAnonymous]
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    // POST: /Account/Login
    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(
        LoginViewModel model,
        CancellationToken token)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            using var response = await _api.PostAsJsonAsync(
                "api/auth/login",
                model,
                token);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Invalid email or password.");

                return View(model);
            }

            AuthResponseViewModel? authResponse;

            try
            {
                authResponse =
                    await response.Content.ReadFromJsonAsync<AuthResponseViewModel>(
                        cancellationToken: token);
            }
            catch (JsonException ex)
            {
                _logger.LogError(
                    ex,
                    "Invalid authentication response from API.");

                ModelState.AddModelError(
                    string.Empty,
                    "Invalid authentication response.");

                return View(model);
            }

            if (authResponse is null ||
                string.IsNullOrWhiteSpace(authResponse.UserId) ||
                string.IsNullOrWhiteSpace(authResponse.Email) ||
                string.IsNullOrWhiteSpace(authResponse.Token) ||
                authResponse.Roles is null ||
                authResponse.Roles.Count == 0)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Invalid authentication response.");

                return View(model);
            }

            // Validate all returned roles.
            var roles = authResponse.Roles
                .Where(role => !string.IsNullOrWhiteSpace(role))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var unsupportedRoles = roles
                .Where(role => !SupportedRoles.Contains(role))
                .ToList();

            if (roles.Count == 0 || unsupportedRoles.Count > 0)
            {
                _logger.LogWarning(
                    "Login returned invalid or unsupported roles: {Roles}",
                    string.Join(", ", unsupportedRoles));

                ModelState.AddModelError(
                    string.Empty,
                    "Your account has invalid or unsupported roles.");

                return View(model);
            }

            // Create MVC Claims.
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, authResponse.UserId),
                new(ClaimTypes.Name, authResponse.Email),
                new(ClaimTypes.Email, authResponse.Email)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            var principal = new ClaimsPrincipal(identity);

            // Authentication Cookie.
            var properties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
            };

            // Store API JWT inside protected authentication ticket.
            properties.StoreTokens(
            [
                new AuthenticationToken
                {
                    Name = "access_token",
                    Value = authResponse.Token
                }
            ]);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                properties);

            return RedirectToAction(
                "Index",
                GetDashboardController(roles));
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(
                ex,
                "Unable to connect to the API during login.");

            ModelState.AddModelError(
                string.Empty,
                "Unable to connect to the API. Please try again later.");

            return View(model);
        }
    }

    // Determine dashboard based on role priority.
    private static string GetDashboardController(
        IEnumerable<string> roles)
    {
        if (roles.Contains("Admin", StringComparer.OrdinalIgnoreCase))
            return "Admin";

        if (roles.Contains("Manager", StringComparer.OrdinalIgnoreCase))
            return "Manager";

        if (roles.Contains("Customer", StringComparer.OrdinalIgnoreCase))
            return "Customer";

        return "Home";
    }

    // POST: /Account/Logout
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction(nameof(Login));
    }

    // GET: /Account/AccessDenied
    [AllowAnonymous]
    [HttpGet]
    public IActionResult AccessDenied()
    {
        return Content(
            "You do not have permission to access this resource.");
    }
}
