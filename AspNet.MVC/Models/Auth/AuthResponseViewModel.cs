namespace AspNet.MVC.Models.Auth;

public class AuthResponseViewModel
{
    public string UserId { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public List<string> Roles { get; set; } = [];

    public string Token { get; set; } = string.Empty;
}