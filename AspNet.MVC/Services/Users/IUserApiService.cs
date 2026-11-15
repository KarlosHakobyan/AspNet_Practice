
using AspNet.MVC.Models.Users;

namespace AspNet.MVC.Services.Users;

public interface IUserApiService
{
    Task<IReadOnlyList<UserListViewModel>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<UserDetailsViewModel?> GetByIdAsync(
        string id,
        CancellationToken cancellationToken = default);

    Task<UserDetailsViewModel?> UpdateAsync(
        string id,
        UserEditViewModel model,
        CancellationToken cancellationToken = default);

    Task<UserDetailsViewModel?> UpdateRolesAsync(
        string id,
        UserRolesViewModel model,
        CancellationToken cancellationToken = default);

    Task<bool> LockAsync(
        string id,
        UserLockViewModel model,
        CancellationToken cancellationToken = default);

    Task<bool> UnlockAsync(
        string id,
        CancellationToken cancellationToken = default);
}
