namespace AspNet.Domain.Models.AuthModels;

public class AuthResponseModel
{
    public string UserId { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public List<string> Roles { get; set; } = [];

    public string Token { get; set; } = string.Empty;
}