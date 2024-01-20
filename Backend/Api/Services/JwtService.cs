using Backend.Api.Contract;
using DatabaseAccessLayer;
using DatabaseAccessLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using Microsoft.Extensions.Options;
using Backend.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Backend.Api.Services
{
    public class JwtService
    {
        private readonly IOptions<JwtAuthenticationOptions> _options;
        public JwtService(IOptions<JwtAuthenticationOptions> options) 
        { 
            _options= options;
        }

        public string GenerateToken(Guid uIdUser, string email, bool isAdmin)
        {
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, uIdUser.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, isAdmin? "admin" : "user")
            }),
                Expires = DateTime.UtcNow.AddMinutes(180),
                Issuer = _options.Value.Issuer,
                Audience = _options.Value.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.Key)), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
       

    }
}
