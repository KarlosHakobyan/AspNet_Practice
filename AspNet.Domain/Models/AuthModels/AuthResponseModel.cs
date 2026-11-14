namespace AspNet.Domain.Models.AuthModels;

public class AuthResponseModel
{
    public string UserId { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string Role { get; set; } = default!;

    public string Token { get; set; } = default!;
}