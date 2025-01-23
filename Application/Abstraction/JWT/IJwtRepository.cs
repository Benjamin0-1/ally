namespace Ally.Application.Abstraction.JWT;

public interface IJwtRepository
{
    Task<int> GetUserIdFromJwt();
    Task<string> GetUserEmailFromJwt();
}
