using AspNet.Data.Identity;

namespace AspNet.Api.Services.Auth
{

    public interface IJwtTokenService
    {
        Task<string> GenerateTokenAsync(ApplicationUser user);
    }
}