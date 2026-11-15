
using AspNet.MVC.Models.Users;
using AspNet.MVC.Services.Users;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNet.MVC.Controllers;

[Authorize(Roles = "Admin")]
public class UsersController(
    IUserApiService userApiService,
    ILogger<UsersController> logger) : Controller
{
    private readonly IUserApiService _userApiService = userApiService;
    private readonly ILogger<UsersController> _logger = logger;

    private static readonly List<string> AvailableRoles =
    [
        "Admin",
        "Manager",
        "Customer"
    ];

    // GET: /Users
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken token)
    {
        try
        {
            var users = await _userApiService.GetAllAsync(token);

            return View(users);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Unable to load users.");

            TempData["Error"] = "Unable to load users.";
            return View(Array.Empty<UserListViewModel>());
        }
    }

    // GET: /Users/Details/{id}
    [HttpGet]
    public async Task<IActionResult> Details(
        string id,
        CancellationToken token)
    {
        try
        {
            var user = await _userApiService.GetByIdAsync(id, token);

            if (user is null)
                return NotFound();

            return View(user);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Unable to load user {UserId}.", id);

            TempData["Error"] = "Unable to load user.";
            return RedirectToAction(nameof(Index));
        }
    }

    // GET: /Users/Edit/{id}
    [HttpGet]
    public async Task<IActionResult> Edit(
        string id,
        CancellationToken token)
    {
        try
        {
            var user = await _userApiService.GetByIdAsync(id, token);

            if (user is null)
                return NotFound();

            return View(new UserEditViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber
            });
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Unable to load user for editing.");

            TempData["Error"] = "Unable to load user.";
            return RedirectToAction(nameof(Index));
        }
    }

    // POST: /Users/Edit/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        string id,
        UserEditViewModel model,
        CancellationToken token)
    {
        if (id != model.Id)
            return BadRequest();

        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var user = await _userApiService.UpdateAsync(id, model, token);

            if (user is null)
                return NotFound();

            TempData["Success"] = "User updated successfully.";

            return RedirectToAction(nameof(Details), new { id });
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Unable to update user {UserId}.", id);

            ModelState.AddModelError(
                string.Empty,
                "Unable to update user. Please try again.");

            return View(model);
        }
    }

    // GET: /Users/Roles/{id}
    [HttpGet]
    public async Task<IActionResult> Roles(
        string id,
        CancellationToken token)
    {
        try
        {
            var user = await _userApiService.GetByIdAsync(id, token);

            if (user is null)
                return NotFound();

            return View(new UserRolesViewModel
            {
                Id = user.Id,
                Email = user.Email,
                AvailableRoles = AvailableRoles,
                SelectedRoles = user.Roles.ToList()
            });
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Unable to load user roles.");

            TempData["Error"] = "Unable to load user roles.";
            return RedirectToAction(nameof(Index));
        }
    }

    // POST: /Users/Roles/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Roles(
        string id,
        UserRolesViewModel model,
        CancellationToken token)
    {
        if (id != model.Id)
            return BadRequest();

        model.AvailableRoles = AvailableRoles;

        if (!ModelState.IsValid)
            return View(model);

        var selectedRoles = model.SelectedRoles
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (selectedRoles.Any(role =>
            !AvailableRoles.Contains(role, StringComparer.OrdinalIgnoreCase)))
        {
            ModelState.AddModelError(
                nameof(model.SelectedRoles),
                "One or more selected roles are invalid.");

            return View(model);
        }

        model.SelectedRoles = selectedRoles;

        try
        {
            var user = await _userApiService.UpdateRolesAsync(
                id,
                model,
                token);

            if (user is null)
                return NotFound();

            TempData["Success"] = "User roles updated successfully.";

            return RedirectToAction(nameof(Details), new { id });
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Unable to update roles for {UserId}.", id);

            ModelState.AddModelError(
                string.Empty,
                "Unable to update user roles. Please try again.");

            return View(model);
        }
    }

    // POST: /Users/Lock/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Lock(
        string id,
        UserLockViewModel model,
        CancellationToken token)
    {
        if (id != model.Id)
            return BadRequest();

        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Invalid lockout period.";
            return RedirectToAction(nameof(Details), new { id });
        }

        try
        {
            var success = await _userApiService.LockAsync(id, model, token);

            if (!success)
                return NotFound();

            TempData["Success"] = "User locked successfully.";
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Unable to lock user {UserId}.", id);

            TempData["Error"] = "Unable to lock user.";
        }

        return RedirectToAction(nameof(Details), new { id });
    }

    // POST: /Users/Unlock/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Unlock(
        string id,
        CancellationToken token)
    {
        try
        {
            var success = await _userApiService.UnlockAsync(id, token);

            if (!success)
                return NotFound();

            TempData["Success"] = "User unlocked successfully.";
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Unable to unlock user {UserId}.", id);

            TempData["Error"] = "Unable to unlock user.";
        }

        return RedirectToAction(nameof(Details), new { id });
    }
}
