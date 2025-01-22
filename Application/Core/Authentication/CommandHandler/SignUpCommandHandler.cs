using Ally.Application.Abstraction.Authentication;
using Ally.Application.Core.Authentication.Command;
using MediatR;

namespace Ally.Application.Core.Authentication.CommandHandler;

public class SignUpCommandHandler : IRequestHandler<SignUpCommand, bool>
{
    private readonly IAuthenticationRepository _authenticationRepository;

    public SignUpCommandHandler(IAuthenticationRepository authenticationRepository)
    {
        _authenticationRepository = authenticationRepository;
    }
    
    // cancellation token not implemented, it will be in the future.
    public async Task<bool> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        try
        {
            return await _authenticationRepository.SignUpAsync(request);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}

