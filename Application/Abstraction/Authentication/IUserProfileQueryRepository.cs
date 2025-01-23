using Ally.Domain.Dtos;

namespace Ally.Application.Abstraction.Authentication;

public interface IUserProfileQueryRepository
{
    public Task<UserProfileDto> GetUserProfileInfo(); // <-- extracted from the token, from argument by an object instead.
}
