using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShootyGameAPI.Database.Entities;
using ShootyGameAPI.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShootyGameAPI.Authorization
{
    public interface IJwtUtils
    {
        public string GenerateJwtToken(User user);
        public int? ValidateJwtToken(string token);
    }
    public class JwtUtils : IJwtUtils
    {
        private readonly AppSettings _appSettings;
        public JwtUtils(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public string GenerateJwtToken(User user)
        {
            JwtSecurityTokenHandler tokenHandler = new();
            byte[] key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(new[] { new Claim("UserId", user.UserId.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public int? ValidateJwtToken(string token)
        {
            if (token == null)
            {
                return null;
            }

            JwtSecurityTokenHandler tokenHandler = new();
            byte[] key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                JwtSecurityToken jwtToken = (JwtSecurityToken)validatedToken;
                int userId = int.Parse(jwtToken.Claims.First(x => x.Type == "UserId").Value);

                return userId;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
