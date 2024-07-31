using CalisanYonetimSistemi.EntityLayer.Concrete;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CalisanYonetimSistemi.WebApiLayer.MyExtensions.Tokens
{
    public class TokenManager
    {

        public async Task<User> CreateToken(User user, IConfiguration configuration)
        {


            List<Claim> Claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Role, user.Rol),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.Ad),
                    new Claim("UserId", user.Id.ToString()),
                    new Claim("Pozisyon", user.Pozisyon)
                };
            user.ExprationToken = DateTime.Now.AddDays(7);

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token:Key"]));   /// Burayı secret dosyasına gönder
            SigningCredentials signingcredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken securityToken = new JwtSecurityToken(
                issuer: "http://localhost:5237",
                audience: "http://localhost:5237",
                expires: user.ExprationToken,
                notBefore: DateTime.Now,
                signingCredentials: signingcredentials,
                claims: Claims


                );

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            user.AccessToken = handler.WriteToken(securityToken);
            user.RefreshToken = await CreateRefreshToken();

            return user;
        }

        public async Task<string> CreateRefreshToken()
        {
            byte[] number = new byte[32];
            using (RandomNumberGenerator random = RandomNumberGenerator.Create())
            {
                random.GetBytes(number);
                return Convert.ToBase64String(number);
            }
        }

    }
}
