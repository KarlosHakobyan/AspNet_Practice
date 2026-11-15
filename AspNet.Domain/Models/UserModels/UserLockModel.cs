namespace AspNet.Domain.Models.Users;

public class UserLockModel
{
    public int LockoutDays { get; set; } = 30;
}