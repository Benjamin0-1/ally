using Ally.Application.Commands.Jwt;  // <-- Add this line to bring GenerateJwtCommand into scope

namespace Ally.Core.Entities.Interfaces.Jwt
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(GenerateJwtCommand request); // <-- Now this will recognize GenerateJwtCommand
    }
}
