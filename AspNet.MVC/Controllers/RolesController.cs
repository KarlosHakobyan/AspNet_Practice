
using AspNet.MVC.Models.Users;
using AspNet.MVC.Services.Users;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNet.MVC.Controllers;

[Authorize(Roles = "Admin")]
public class RolesController(
    IUserApiService userApiService,
    ILogger<RolesController> logger) : Controller
{
    private readonly IUserApiService _userApiService = userApiService;
    private readonly ILogger<RolesController> _logger = logger;

    private static readonly string[] SystemRoles =
    [
        "Admin",
        "Manager",
        "Customer"
    ];

    // GET: /Roles
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken token)
    {
        try
        {
            var users = await _userApiService.GetAllAsync(token);

            var roles = SystemRoles
                .Select(role => new RoleDetailsViewModel
                {
                    Name = role,
                    Users = users
                        .Where(user => user.Roles.Contains(
                            role,
                            StringComparer.OrdinalIgnoreCase))
                        .ToList()
                })
                .ToList();

            return View(roles);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Unable to load roles.");

            TempData["Error"] = "Unable to load roles.";

            return View(new List<RoleDetailsViewModel>());
        }
    }

    // GET: /Roles/Details/{role}
    [HttpGet]
    public async Task<IActionResult> Details(
        string id,
        CancellationToken token)
    {
        if (!SystemRoles.Contains(
            id,
            StringComparer.OrdinalIgnoreCase))
        {
            return NotFound();
        }

        try
        {
            var users = await _userApiService.GetAllAsync(token);

            var role = new RoleDetailsViewModel
            {
                Name = id,
                Users = users
                    .Where(user => user.Roles.Contains(
                        id,
                        StringComparer.OrdinalIgnoreCase))
                    .ToList()
            };

            return View(role);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Unable to load role {Role}.", id);

            TempData["Error"] = "Unable to load role.";

            return RedirectToAction(nameof(Index));
        }
    }
}
