using Ally.Domain.Dtos;
using MediatR;

namespace Ally.Application.Core.Authentication.Query;

public class GetUserProfileQuery : IRequest<UserProfileDto>
{
    // none.
}
