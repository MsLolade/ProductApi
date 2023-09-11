using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProductApi
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _appSettings;

        public TokenService(IOptions<JwtSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        public string GenerateAccessToken()
        {
            
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(
                   issuer: _appSettings.Issuer,
                   audience: _appSettings.Audience,
                   notBefore: DateTime.UtcNow,
                   expires: DateTime.UtcNow.AddDays(7),
                   signingCredentials: signinCredentials
               );

            return new JwtSecurityTokenHandler().WriteToken(tokeOptions);

            //the method is called WriteToken but returns a string
        }
    }
}
