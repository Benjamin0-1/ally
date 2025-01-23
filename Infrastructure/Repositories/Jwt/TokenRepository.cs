using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Ally.Application.Abstraction.Authentication;
using Ally.Domain.Dtos;
using Ally.Infrastructure.Data;
using Microsoft.IdentityModel.Tokens;

namespace Ally.Infrastructure.Repositories.Jwt;

public class TokenRepository : ITokenRepository
{
    private readonly ApplicationDbContext _context;
    // use IConfiguration to grab the jwt settings from there.
    private readonly string _secretKey = "your-secret-keyadasdasdsaaaaaddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd";

    public TokenRepository(ApplicationDbContext applicationDbContext)
    {
        _context = applicationDbContext;
    }

    public async Task<string> GenerateToken(UserDto user)
    {
        try
        {
            var claims = GenerateTokenClaims(user);

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(240),
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
            var jwtHandler = new JwtSecurityTokenHandler();
            
            // create the claims using the dto
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) ,
                new Claim(ClaimTypes.Email, user.Email) ,
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



/**
 *   var claims = new List<Claim> // <-- accessClaims
  {
     new Claim("Id",user.Id.ToString()), 
      //new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()) ,
      new Claim(ClaimTypes.NameIdentifier, user.UserName) ,
      new Claim(ClaimTypes.Email, user.Email) ,
      new Claim(ClaimTypes.GivenName, user.Name),
      new Claim(ClaimTypes.Role, user.Role) ,
      new Claim("RoleId", user.RoleId.ToString()),
      new Claim("TokenType", "access")
  };
 */




















