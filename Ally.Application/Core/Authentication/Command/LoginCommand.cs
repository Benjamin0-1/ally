using MediatR;

namespace Ally.Application.Core.Authentication.Command;

public class LoginCommand : IRequest<string> // string for testing, will actually return a dto
{
    public string Email { get; set; }
    
    public string Password { get; set; }
}
