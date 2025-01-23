using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Ally.Application.Abstraction.Authentication;
using Ally.Domain.Dtos;
using Ally.Infrastructure.Data;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Text;
using System;

namespace Ally.Infrastructure.Repositories.Jwt
{
    public class TokenRepository : ITokenRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        // Constructor with IConfiguration to grab the jwt settings
        public TokenRepository(ApplicationDbContext applicationDbContext, IConfiguration configuration)
        {
            _context = applicationDbContext;
            _configuration = configuration;
        }

        public async Task<string> GenerateToken(UserDto user)
        {
            try
            {
                var jwtSettings = _configuration.GetSection("JwtSettings");

                var claims = new List<Claim>
                {
                    // Changed 'nameid' to 'Id' here
                    new Claim("Id", user.Id.ToString()),  // <-- Changed from ClaimTypes.NameIdentifier to "Id"
                    new Claim(ClaimTypes.Email, user.Email),
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddHours(Convert.ToDouble(jwtSettings["ExpiresInHours"])),
                    Issuer = jwtSettings["Issuer"],
                    Audience = jwtSettings["Audience"],
                    SigningCredentials = creds
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public List<Claim> GenerateTokenClaims(UserDto user)
        {
            try
            {
                var claims = new List<Claim>
                {
                    // Changed 'nameid' to 'Id' here
                    new Claim("Id", user.Id.ToString()),  // <-- Changed from ClaimTypes.NameIdentifier to "Id"
                    new Claim(ClaimTypes.Email, user.Email)
                };

                return claims;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
