
using AspNet.BLL.Common.Results;

namespace AspNet.BLL.Services.Auth;

public interface IRoleService
{
    Task<Result<IReadOnlyList<string>>> GetAllRolesAsync(CancellationToken token);

    Task<Result<IReadOnlyList<string>>> GetUserRolesAsync(string userId,CancellationToken token);

    Task<Result> AssignRoleAsync(string userId,string roleName,CancellationToken token);

    Task<Result> RemoveRoleAsync(string userId,string roleName,CancellationToken token);
}
