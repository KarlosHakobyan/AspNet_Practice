
using System.ComponentModel.DataAnnotations;

namespace AspNet.MVC.Models.Users;

public class UserRolesViewModel
{
    [Required]
    public string Id { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public List<string> AvailableRoles { get; set; } = [];

    public List<string> SelectedRoles { get; set; } = [];
}
