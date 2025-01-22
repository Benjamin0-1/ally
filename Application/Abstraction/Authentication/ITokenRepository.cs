using System.Security.Claims;
using Ally.Domain.Dtos;

namespace Ally.Application.Abstraction.Authentication;

public interface ITokenRepository
{
    Task<string> GenerateToken(UserDto user);
    List<Claim> GenerateTokenClaims(UserDto user);
}

