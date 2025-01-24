using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Ally.Application.Abstraction.Authentication;
using Ally.Domain.Dtos;
using Ally.Infrastructure.Repositories.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace Ally.Tests.Repositories.Token;

/**
 * Currently it does not test for the RoleId claims nor roleName
 * since the ActionFilter for it is actually checking it against the database.
 */

public class TokenRepositoryTest
{
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly ITokenRepository _tokenRepository;
    private readonly UserDto _testUser;

    public TokenRepositoryTest() // q: why is there no constructor injection here?
    {
        _mockConfiguration = new Mock<IConfiguration>();

        _mockConfiguration
            .Setup(config => config.GetSection("JwtSettings")["SecretKey"])
            .Returns("fG7!bW$2XpQ9#9r8@kzA^9rVjD6u3uLz\n");  // Match real SecretKey

        _mockConfiguration
            .Setup(config => config.GetSection("JwtSettings")["ExpiresInHours"])
            .Returns("10");  // Match real ExpiresInHours

        _mockConfiguration
            .Setup(config => config.GetSection("JwtSettings")["Issuer"])
            .Returns("Ally-issuer");  // Match real Issuer

        _mockConfiguration
            .Setup(config => config.GetSection("JwtSettings")["Audience"])
            .Returns("Ally-audience");  // Match real Audience


        _testUser = new UserDto
        {
            Id = 1,
            Email = "Example@gmail.com"
        };

        _tokenRepository = new TokenRepository(null, _mockConfiguration.Object); // <-- pass object to the configuration on this repository, as defined there.
    }

    [Fact]
    public async Task GenerateTokenClaims_ShouldReturnCorrectClaims()
    {
        var claims = _tokenRepository.GenerateTokenClaims(_testUser);

        Assert.NotNull(claims);
        Assert.Contains(claims, c => c.Type == "Id" && c.Value == _testUser.Id.ToString());
        Assert.Contains(claims, c => c.Type == ClaimTypes.Email && c.Value == _testUser.Email);
    }

    
    [Fact]
    public async Task GenerateToken_ShouldReturnValidToken()
    {
        var token = await _tokenRepository.GenerateToken(_testUser);
        
        Assert.NotNull(token);  
        
        var tokenHandler = new JwtSecurityTokenHandler();
        
        var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
        Assert.NotNull(jwtToken);
        
        Assert.Equal("Ally-issuer", jwtToken.Issuer);  
        Assert.Equal("Ally-audience", jwtToken.Audiences.First());  
        Assert.True(jwtToken.ValidTo > DateTime.UtcNow);
        
        var idClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == "Id");
        Assert.NotNull(idClaim);
        Assert.Equal(_testUser.Id.ToString(), idClaim?.Value);

        var emailClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == "email");  
        Assert.NotNull(emailClaim);  
        Assert.Equal(_testUser.Email, emailClaim?.Value); 
    }



    [Fact]
    public void GenerateToken_ShouldReturnNull_WhenExceptionOccurs()
    {
        _mockConfiguration
            .Setup(config => config.GetSection("JwtSettings")["SecretKey"])
            .Returns((string)null); // Simulate invalid configuration 

        var result = _tokenRepository.GenerateToken(_testUser).Result;
        
        Assert.Null(result);
    }
}















