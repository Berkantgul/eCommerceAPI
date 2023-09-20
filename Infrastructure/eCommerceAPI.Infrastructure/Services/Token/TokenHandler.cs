using eCommerceAPI.Application.Abstractions.Token;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Infrastructure.Services.Token
{
    public class TokenHandler : ITokenHandler
    {
        readonly IConfiguration _configuration;

        public TokenHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public Application.DTOs.Token CreateToken(int seconds)
        {
            //DTOs içeriisndeki token nesnemi bağlıyorum.
            Application.DTOs.Token token = new();

            //Symetric key oluşturacağım
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));

            // Şifrelenmiş kimliği oluşturuyorum
            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            // Oluşturulacak token ayarlarını tanımlıyorum
            token.Expiration = DateTime.UtcNow.AddSeconds(seconds);
            JwtSecurityToken securityToken = new(
                    audience: _configuration["Token:Audience"],
                    issuer: _configuration["Token:Issuer"],
                    expires: token.Expiration,
                    notBefore: DateTime.UtcNow,
                    signingCredentials: signingCredentials
                );
            // Token oluşturucu sınıfından bir örnek alıyorum
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            token.AccessToken = tokenHandler.WriteToken(securityToken);

            // Create Refresh Token
            token.RefreshToken = CreateRefreshToken();
            return token;
        }
       
        public string CreateRefreshToken()
        {
            byte[] number = new byte[32];
            using RandomNumberGenerator random = RandomNumberGenerator.Create();
            random.GetBytes(number);
            return Convert.ToBase64String(number);
            // CreateAccessToken fonksiyonu tetiklendiğinde çağırmak istiyorum
        }
    }
}
