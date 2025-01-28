using Moq;
using Xunit;
using Ally.Infrastructure.User;
using Ally.Application.Core.Authentication.Command;
using Ally.Domain.Dtos;
using Ally.Infrastructure.Data;
using Ally.Application.Abstraction.JWT;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using Ally.Application.Abstraction.Authentication;
// using System.Collections; // <-- non type safe
using System.Collections.Generic; // <-- type safe
// new ArrayList()

namespace Ally.Tests.Repositories.Authentication;

public class UserCommandRepositoryTests
{
    private readonly Mock<IAuthenticationRepository> _mockAuthenticationRepository;
    private readonly Mock<ITokenRepository> _mockTokenRepository;
    private readonly ApplicationDbContext _context;
    private readonly UserCommandRepository _userCommandRepository;

    public UserCommandRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "AllyTestDb")
            .Options;

        _context = new ApplicationDbContext(options);
        _mockAuthenticationRepository = new Mock<IAuthenticationRepository>();
        _mockTokenRepository = new Mock<ITokenRepository>();

        _userCommandRepository = new UserCommandRepository(_context, _mockTokenRepository.Object, null);
    }

    [Fact]
    public async Task SignUpAsync_ShouldReturnTrue_WhenUserIsCreatedSuccessfully()
    {
        var signUpCommand = new SignUpCommand
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@example.com",
            Password = "Password123",
            ConfirmPassword = "Password123"
        };

        var result = await _userCommandRepository.SignUpAsync(signUpCommand);

        Assert.True(result);

        // now check the user is in the database
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == signUpCommand.Email);
        Assert.NotNull(user);
        Assert.Equal(signUpCommand.Email, user.Email);
        Assert.Equal(signUpCommand.FirstName, user.FirstName);
        Assert.Equal(signUpCommand.LastName, user.LastName);
    }


    [Fact]
    public async Task SignUpAsync_ShouldReturnFalse_WhenUserAlreadyExists()
    {
        var signUpCommand1 = new SignUpCommand
        {
            FirstName = "Jane",
            LastName = "Doe",
            Email = "janedoe@example.com",
            Password = "Password123",
            ConfirmPassword = "Password123"
        };

        var result = await _userCommandRepository.SignUpAsync(signUpCommand1);

        Assert.False(result); // <-- FAILED. RUN MIGRATIONS, COnfirmpassword is still being required at a db level.
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnLoginDto_WhenUserCredentialsAreValid()
    {
        var signUpCommand = new SignUpCommand
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@example.com",
            Password = "Password123",
            ConfirmPassword = "Password123"
        };

        await _userCommandRepository.SignUpAsync(signUpCommand);

        var loginCommand = new LoginCommand
        {
            Email = "johndoe@example.com",
            Password = "Password123"
        };

        _mockTokenRepository.Setup(x => x.GenerateToken(It.IsAny<UserDto>()))
            .ReturnsAsync("FakeJWTToken");

        var loginResult = await _userCommandRepository.LoginAsync(loginCommand);

        Assert.NotNull(loginResult);
        Assert.Equal("FakeJWTToken", loginResult.Token);
    }

    [Fact]
    public async Task LoginAsync_ShouldThrowException_WhenUserDoesNotExist()
    {
        var loginCommand = new LoginCommand
        {
            Email = "nonexistent@example.com",
            Password = "Password123"
        };
        
        var exception = await Assert.ThrowsAsync<Exception>(() => _userCommandRepository.LoginAsync(loginCommand));
        Assert.Equal("User does not exist", exception.Message);
    }
    
    [Fact]
    public async Task LoginAsync_ShouldThrowException_WhenPasswordIsIncorrect()
    {

        var signUpCommand = new SignUpCommand
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@example.com",
            Password = "Password123",
            ConfirmPassword = "Password123"
        };
            
        await _userCommandRepository.SignUpAsync(signUpCommand);

        var loginCommand = new LoginCommand
        {
            Email = "johndoe@example.com",
            Password = "WrongPassword"
        };
 
        var exception = await Assert.ThrowsAsync<ApplicationException>(() => _userCommandRepository.LoginAsync(loginCommand)); // <- make it match : System.Exception: Error: System.Exception: Wrong credentials
        Assert.Equal("Wrong credentials", exception.Message);
    }
}


















