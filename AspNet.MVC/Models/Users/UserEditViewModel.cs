
using System.ComponentModel.DataAnnotations;

namespace AspNet.MVC.Models.Users;

public class UserEditViewModel
{
    [Required]
    public string Id { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Phone]
    public string? PhoneNumber { get; set; }
}
