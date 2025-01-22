using Ally.Application.Abstraction.Authentication;
using Ally.Application.Core.Authentication.Command;
using Ally.Domain.Dtos;
using MediatR;
//using Serilog <-- install.

namespace Ally.Application.Core.Authentication.CommandHandler;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginDto>
{
    private readonly IAuthenticationRepository _authenticationRepository;

    public LoginCommandHandler(IAuthenticationRepository authenticationRepository)
    {
        _authenticationRepository = authenticationRepository;
    }

    public async Task<LoginDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        try
        {
            return await _authenticationRepository.LoginAsync(request);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception(e.Message);
        }
    }
}


