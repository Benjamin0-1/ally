using Ally.Application.Abstraction.Authentication;
using Ally.Application.Abstraction.JWT;
using Ally.Domain.Dtos;
using Ally.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ally.Infrastructure.User
{
    public class UserProfileQueryRepository : IUserProfileQueryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtRepository _jwtRepository;

        public UserProfileQueryRepository(ApplicationDbContext context,
            IJwtRepository jwtRepository)
        {
            _context = context;
            _jwtRepository = jwtRepository;
        }
    
        public async Task<UserProfileDto> GetUserProfileInfo()
        {
            try
            {
                int userId = await _jwtRepository.GetUserIdFromJwt();
                
                Console.WriteLine($"User ID : {userId}");
                
                var user = await _context.Users
                    .Where(x => x.Id == userId)
                    .FirstOrDefaultAsync();
                
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                
                var role = await _context.Roles
                    .Where(r => r.Id == user.RoleId)
                    .FirstOrDefaultAsync();
                
                /**
                 * Avoid manually creating an instance here.
                 */
                return new UserProfileDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    RoleId = user.RoleId,
                    RoleName = user.Role.Name ?? ""
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
