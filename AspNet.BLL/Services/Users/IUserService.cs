
using AspNet.BLL.Common.Results;
using AspNet.Domain.Models.Users;

namespace AspNet.BLL.Services.Users;

public interface IUserService
{
    Task<Result<IReadOnlyList<UserListModel>>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<Result<UserDetailsModel>> GetByIdAsync(
        string userId,
        CancellationToken cancellationToken = default);

    Task<Result<UserDetailsModel>> UpdateAsync(
        string userId,
        UserUpdateModel model,
        CancellationToken cancellationToken = default);

    Task<Result<UserDetailsModel>> UpdateRolesAsync(
        string userId,
        UserRolesUpdateModel model,
        string currentAdminId,
        CancellationToken cancellationToken = default);

    Task<Result<UserDetailsModel>> LockAsync(
        string userId,
        UserLockModel model,
        string currentAdminId,
        CancellationToken cancellationToken = default);

    Task<Result<UserDetailsModel>> UnlockAsync(
        string userId,
        CancellationToken cancellationToken = default);
}
