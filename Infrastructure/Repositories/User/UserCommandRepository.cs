using Ally.Application.Abstraction.Authentication;
using Ally.Application.Core.Authentication.Command;
using Ally.Domain.Dtos;
using System.Threading.Tasks;

namespace Ally.Infrastructure.User
{
    public class UserCommandRepository : IAuthenticationRepository
    {
        // Implement LoginAsync to return a LoginDto
        public async Task<LoginDto> LoginAsync(LoginCommand request)
        {
            // Your login logic here.
            // This is a placeholder implementation; replace it with your actual logic for authentication.
            
            // For demonstration, we're returning a dummy LoginDto object.
            return await Task.FromResult(new LoginDto
            {
                Role = ""
            });
        }
    }
}

