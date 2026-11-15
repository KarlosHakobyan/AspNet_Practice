
using System.ComponentModel.DataAnnotations;

namespace AspNet.MVC.Models.Users;

public class UserLockViewModel
{
    [Required]
    public string Id { get; set; } = string.Empty;

    [Range(1, 365)]
    public int LockoutDays { get; set; } = 30;
}
