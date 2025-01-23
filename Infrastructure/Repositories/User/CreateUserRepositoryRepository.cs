using Ally.Application.Abstraction.Authentication;
using Ally.Application.Core.Authentication.Command;
using Ally.Domain.Entities;

namespace Ally.Infrastructure.User;

public class CreateUserRepositoryRepository : ICreateUserRepository
{
    public UserEntity CreateUser(UserEntity request, string hashedPassword)
    {
        try
        {
            return new UserEntity
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Password = hashedPassword, // <-- necessary.
                RoleId = request.RoleId
            };
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
