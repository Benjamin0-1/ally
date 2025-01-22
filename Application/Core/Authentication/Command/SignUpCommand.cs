using MediatR;

namespace Ally.Application.Core.Authentication.Command;

public class SignUpCommand : IRequest<bool>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public string Email { get; set; } // <-- fluent valdiation / regex / check contraint on table directly.
    
    public string Password { get; set; } // <-- fluent validation / regex
    public string ConfirmPassword { get; set; }
}

