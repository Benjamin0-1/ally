using Ally.Application.Abstraction.Authentication;
using Ally.Application.Core.Authentication.Command;
using Ally.Domain.Dtos;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net; // <-- hashing password
using System.Threading.Tasks;
using Ally.Domain.Entities;
using Ally.Infrastructure.Data;
using Microsoft.Extensions.Configuration;


namespace Ally.Infrastructure.User
{
    public class UserCommandRepository : IAuthenticationRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ITokenRepository _tokenRepository;
        private readonly ICreateUserRepository _createUserRepository;
        private readonly IConfiguration _configuration;
        public UserCommandRepository(ApplicationDbContext applicationDbContext,
            ITokenRepository tokenRepository,
            ICreateUserRepository createUserRepository,
            IConfiguration configuration)
        {
            _context = applicationDbContext;
            _tokenRepository = tokenRepository;
            _createUserRepository = createUserRepository;
            _configuration = configuration;
        }

        public async Task<bool> SignUpAsync(SignUpCommand request)
        {
            try
            {
                var existingUser = await _context.Users
                    .Where(x => x.Email == request.Email)
                    .FirstOrDefaultAsync();

                if (existingUser != null)
                {
                    return false;
                }
                
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
                
                
                // the below default roles are only temporary.
                var defaultRole = await _context.Roles.FirstOrDefaultAsync(r => r.Id == 0);
                if (defaultRole == null)
                {
                    defaultRole = new RoleEntity { Name = "User" }; 
                    _context.Roles.Add(defaultRole);
                    await _context.SaveChangesAsync();
                }
                
                var newUser = MapToUserEntity(request, hashedPassword, defaultRole.Id);
                
                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
                if (e.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {e.InnerException.Message}");
                }
                return false;
            }
        }

        
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

        public bool UserPasswordConfirmation(string Password, string ConfirmPassword)
        {
            return Password == ConfirmPassword;
        }
        
        
        /**
         * Later move the method below to its own interface to follow SRP.
         */
        private UserEntity MapToUserEntity(SignUpCommand request, string hashedPassword, int roleId)
        {
            return new UserEntity
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Password = hashedPassword, 
                RoleId = roleId
            };
        }

    }
}


































































