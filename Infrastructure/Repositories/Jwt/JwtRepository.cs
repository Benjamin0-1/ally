using Ally.Application.Abstraction.JWT;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http; 

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;


namespace Ally.Infrastructure.Repositories.Jwt;

public class JwtRepository : IJwtRepository
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public JwtRepository(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<int> GetUserIdFromJwt()
    {
        try
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;
            
            Console.WriteLine($"UserId Claim: {userIdClaim}");

            if (string.IsNullOrEmpty(userIdClaim))
            {
                throw new Exception("Id claim is missing or empty.");
            }

            int userId = Convert.ToInt32(userIdClaim);
            return userId;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }


    public async Task<string> GetUserEmailFromJwt()
    {
        try
        {
            string email = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "email")?.Value;
            return email;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
