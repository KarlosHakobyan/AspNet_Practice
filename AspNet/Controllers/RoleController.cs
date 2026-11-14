
using AspNet.Api.Extensions;
using AspNet.BLL.Services.Auth;
using AspNet.Data.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNet.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = RoleNames.Admin)]
public class RoleController(IRoleService roleService) : ControllerBase
{
    private readonly IRoleService _roleService = roleService;

    // GET: api/role
    [HttpGet]
    public async Task<IActionResult> GetAllRoles(CancellationToken token)
    {
        var result = await _roleService.GetAllRolesAsync(token);

        return this.ToActionResult(
            result,
            roles => Ok(roles));
    }

    // GET: api/role/user/{userId}
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserRoles(
        [FromRoute] string userId,
        CancellationToken token)
    {
        var result = await _roleService.GetUserRolesAsync(userId, token);

        return this.ToActionResult(
            result,
            roles => Ok(roles));
    }

    // POST: api/role/assign/{userId}/{roleName}
    [HttpPost("assign/{userId}/{roleName}")]
    public async Task<IActionResult> AssignRole(
        [FromRoute] string userId,
        [FromRoute] string roleName,
        CancellationToken token)
    {
        var result = await _roleService.AssignRoleAsync(
            userId, roleName, token);

        return this.ToActionResult(result);
    }

    // DELETE: api/role/remove/{userId}/{roleName}
    [HttpDelete("remove/{userId}/{roleName}")]
    public async Task<IActionResult> RemoveRole(
        [FromRoute] string userId,
        [FromRoute] string roleName,
        CancellationToken token)
    {
        var result = await _roleService.RemoveRoleAsync(
            userId, roleName, token);

        return this.ToActionResult(result);
    }
}
