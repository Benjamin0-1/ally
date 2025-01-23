using Ally.Application.Abstraction.Authentication;
using Ally.Application.Core.Authentication.Query;
using Ally.Domain.Dtos;
using MediatR;

namespace Ally.Application.Core.Authentication.QueryHandler;

public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UserProfileDto>
{
    private readonly IUserProfileQueryRepository _repository;

    public GetUserProfileQueryHandler(IUserProfileQueryRepository userProfileQueryRepository)
    {
        _repository = userProfileQueryRepository;
    }

    public async Task<UserProfileDto> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        try
        {
            return await _repository.GetUserProfileInfo();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
