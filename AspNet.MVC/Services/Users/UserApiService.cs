
using System.Net;
using System.Net.Http.Json;

using AspNet.MVC.Models.Users;

namespace AspNet.MVC.Services.Users;

public class UserApiService(HttpClient httpClient) : IUserApiService
{
    public async Task<IReadOnlyList<UserListViewModel>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var users = await httpClient.GetFromJsonAsync<List<UserListViewModel>>(
            "api/users",
            cancellationToken);

        return users ?? [];
    }

    public async Task<UserDetailsViewModel?> GetByIdAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.GetAsync(
            $"api/users/{Uri.EscapeDataString(id)}",
            cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<UserDetailsViewModel>(
            cancellationToken: cancellationToken);
    }

    public async Task<UserDetailsViewModel?> UpdateAsync(
        string id,
        UserEditViewModel model,
        CancellationToken cancellationToken = default)
    {
        var request = new
        {
            model.FirstName,
            model.LastName,
            model.PhoneNumber
        };

        using var response = await httpClient.PutAsJsonAsync(
            $"api/users/{Uri.EscapeDataString(id)}",
            request,
            cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<UserDetailsViewModel>(
            cancellationToken: cancellationToken);
    }

    public async Task<UserDetailsViewModel?> UpdateRolesAsync(
        string id,
        UserRolesViewModel model,
        CancellationToken cancellationToken = default)
    {
        var request = new
        {
            Roles = model.SelectedRoles
        };

        using var response = await httpClient.PutAsJsonAsync(
            $"api/users/{Uri.EscapeDataString(id)}/roles",
            request,
            cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<UserDetailsViewModel>(
            cancellationToken: cancellationToken);
    }

    public async Task<bool> LockAsync(
        string id,
        UserLockViewModel model,
        CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.PutAsJsonAsync(
            $"api/users/{Uri.EscapeDataString(id)}/lock",
            new { model.LockoutDays },
            cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return false;

        response.EnsureSuccessStatusCode();

        return true;
    }

    public async Task<bool> UnlockAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.PutAsync(
            $"api/users/{Uri.EscapeDataString(id)}/unlock",
            null,
            cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return false;

        response.EnsureSuccessStatusCode();

        return true;
    }
}
