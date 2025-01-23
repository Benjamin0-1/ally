using Ally.Application.Core.Authentication.Command;
using Ally.Domain.Entities;

namespace Ally.Application.Abstraction.Authentication;

public interface ICreateUserRepository
{
    UserEntity CreateUser(UserEntity request, string hashedPassword); // <-- replace hashedPassword for a new command that has both in it.
}
