namespace AspNet.Domain.Models.Users;

public class UserDetailsModel
{
    public string Id { get; set; } = default!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string Email { get; set; } = default!;

    public string? PhoneNumber { get; set; }

    public IList<string> Roles { get; set; } = [];

    public bool EmailConfirmed { get; set; }

    public bool IsLockedOut { get; set; }

    public DateTimeOffset? LockoutEnd { get; set; }
}