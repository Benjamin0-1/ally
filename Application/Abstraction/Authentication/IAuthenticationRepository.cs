using Ally.Application.Core.Authentication.Command;
using Ally.Domain.Dtos;
using System.Threading.Tasks;

namespace Ally.Application.Abstraction.Authentication
{
    public interface IAuthenticationRepository
    {
    //    Task<bool> SignUpAsync(SignUpCommand request); // <-- replace bool by dto so the user directly logs in at the same time.
        Task<LoginDto> LoginAsync(LoginCommand request);
    }
}