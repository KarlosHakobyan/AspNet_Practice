
namespace AspNet.MVC.Models.Users;

public class RoleDetailsViewModel
{
    public string Name { get; set; } = string.Empty;

    public List<UserListViewModel> Users { get; set; } = [];

    public int UserCount => Users.Count;
}
