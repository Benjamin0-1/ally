using Ally.Application.Core.Authentication.Command;
using Ally.Domain.Dtos;
using System.Threading.Tasks;

namespace Ally.Application.Abstraction.Authentication
{
    public interface IAuthenticationRepository
    {
        Task<LoginDto> LoginAsync(LoginCommand request);
    }
}