using Ally.Application.Abstraction.Authentication;
using Ally.Application.Core.Authentication.Command;
using Ally.Domain.Dtos;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net; // <-- hashing password
using System.Threading.Tasks;
using Ally.Infrastructure.Data;


namespace Ally.Infrastructure.User
{
    public class UserCommandRepository : IAuthenticationRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ITokenRepository _tokenRepository;
        public UserCommandRepository(ApplicationDbContext applicationDbContext,
            ITokenRepository tokenRepository)
        {
            _context = applicationDbContext;
            _tokenRepository = tokenRepository;
        }
        
        // wrap in try catch.
        public async Task<LoginDto> LoginAsync(LoginCommand request)
        {

            try
            {
                var user = await _context.Users.Where(x => x.Email == request.Email)
                    .FirstOrDefaultAsync(); // <- find by the email
                if (user == null)
                {
                    throw new Exception("User does not exist");
                }
                
                if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                {
                    throw new Exception("Wrong credentials");
                }

                var userDto = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email
                };

                var token = await _tokenRepository.GenerateToken(userDto);
                
                return new LoginDto
                {
                    Token = token,
                    TokenExpiresIn = DateTime.UtcNow.AddHours(1)
                };
            }

            catch(Exception ex)
            {
                Console.WriteLine($"Error during login: {ex.Message}");
                throw new ApplicationException($"Error: {ex}");
            }
        }
    }
}


































































