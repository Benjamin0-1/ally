using Ally.Domain.Dtos;
using MediatR;

namespace Ally.Application.Core.Authentication.Command;

public class LoginCommand : IRequest<LoginDto>
{
    public string Email { get; set; }
    
    public string Password { get; set; }
}
