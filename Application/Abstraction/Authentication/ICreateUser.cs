using Ally.Application.Core.Authentication.Command;

namespace Ally.Application.Abstraction.Authentication;

public interface ICreateUser
{
    Task<bool> CreateUser(SignUpCommand request, string hashedPassword); // <-- replace hashedPassword for a new command that has both in it.
}