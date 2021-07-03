using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace DotNet_Dojo_RafaelMarcos.Models
{
    public static class JWT
    {
        public class Token
        {
            public string Jwt { get; set; }
            public double ExpiresIn { get; set; }
        }
        
        private static ICollection<Claim> _userClaims;
        
        public static Token BuildUserResponse(User user)
        {
            _userClaims = new List<Claim>()
            {
                new(ClaimTypes.Role, user.Role),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Name, user.Name),
            };
            
            var resp = new Token
            {
                Jwt = BuildToken(),
                ExpiresIn = TimeSpan.FromHours(Constant.JwtSettings.Expiration).TotalSeconds,
            };

            return resp;
        }
        
        private static string BuildToken()
        {
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaims(_userClaims);
         
            var tokenHandler = new JwtSecurityTokenHandler();
            var issuerSigningKey = Encoding.ASCII.GetBytes(Constant.JwtSettings.IssuerSigningKey);
            var tokenDecryptionKey = Encoding.ASCII.GetBytes(Constant.JwtSettings.TokenDecryptionKey);
            
            var token = tokenHandler.CreateJwtSecurityToken(new SecurityTokenDescriptor
            {
                Issuer = Constant.JwtSettings.Issuer,
                Audience = Constant.JwtSettings.Audience,
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddHours(Constant.JwtSettings.Expiration),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(issuerSigningKey),
                    SecurityAlgorithms.HmacSha256Signature),
                EncryptingCredentials = new EncryptingCredentials(
                    new SymmetricSecurityKey(tokenDecryptionKey),
                    SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256),
            });

            return tokenHandler.WriteToken(token);
        }
    }
}