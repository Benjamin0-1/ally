using Ally.Application.Abstraction.JWT;
using Microsoft.EntityFrameworkCore;
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
            int userId = Convert.ToInt32(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "nameid")?.Value);
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
