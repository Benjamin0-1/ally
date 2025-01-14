using MediatR; // <-- sln.

namespace Ally.Application.Commands.Jwt;

public class GenerateJwtCommand : IRequest<string>
{
      public int UserId {get;}
      public string Email {get;}
      public string[] Roles {get;}

      public GenerateJwtCommand(int userId, string email, string[] roles)
      {
        UserId  = userId;
        Email = email;
        Roles = roles;
      }
}
