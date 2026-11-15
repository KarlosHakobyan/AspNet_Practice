
using System.Security.Claims;

using AspNet.Api.Extensions;
using AspNet.BLL.Services.Users;
using AspNet.Domain.Models.Users;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNet.Api.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
[ApiController]
public class UsersController(
    IUserService userService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var result = await userService.GetAllAsync(cancellationToken);

        return this.ToActionResult(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(
        string id,
        CancellationToken cancellationToken)
    {
        var result = await userService.GetByIdAsync(
            id,
            cancellationToken);

        return this.ToActionResult(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        string id,
        [FromBody] UserUpdateModel model,
        CancellationToken cancellationToken)
    {
        var result = await userService.UpdateAsync(
            id,
            model,
            cancellationToken);

        return this.ToActionResult(result);
    }

    [HttpPut("{id}/roles")]
    public async Task<IActionResult> UpdateRoles(
        string id,
        [FromBody] UserRolesUpdateModel model,
        CancellationToken cancellationToken)
    {
        var currentAdminId = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(currentAdminId))
            return Unauthorized();

        var result = await userService.UpdateRolesAsync(
            id,
            model,
            currentAdminId,
            cancellationToken);

        return this.ToActionResult(result);
    }

    [HttpPut("{id}/lock")]
    public async Task<IActionResult> Lock(
        string id,
        [FromBody] UserLockModel model,
        CancellationToken cancellationToken)
    {
        var currentAdminId = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(currentAdminId))
            return Unauthorized();

        var result = await userService.LockAsync(
            id,
            model,
            currentAdminId,
            cancellationToken);

        return this.ToActionResult(result);
    }

    [HttpPut("{id}/unlock")]
    public async Task<IActionResult> Unlock(
        string id,
        CancellationToken cancellationToken)
    {
        var result = await userService.UnlockAsync(
            id,
            cancellationToken);

        return this.ToActionResult(result);
    }
}
