
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using AspNet.Api.Settings;
using AspNet.Data.Identity;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AspNet.Api.Services.Auth
{

    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtSettings _settings;
        private readonly UserManager<ApplicationUser> _userManager;

        public JwtTokenService(
            IOptions<JwtSettings> options,
            UserManager<ApplicationUser> userManager)
        {
            _settings = options.Value;
            _userManager = userManager;
        }

        public async Task<string> GenerateTokenAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName!)
        };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_settings.SecretKey)
            );

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    _settings.ExpirationInMinutes
                ),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}