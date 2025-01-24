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
            .Returns("SuperSecretKeyForJwt"); // <-- simply a test secret.

        _mockConfiguration
            .Setup(config => config.GetSection("JwtSettings")["ExpiresInHours"])
            .Returns("1");
        
        _mockConfiguration 
            .Setup(config => config.GetSection("JwtSettings")["Issuer"])
            .Returns("TestIssuer");

        _mockConfiguration
            .Setup(config => config.GetSection("JwtSettings")["Audience"])
            .Returns("TestAudience");

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
        
        Assert.NotNull(token); // <-- test FAILED.
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

        Assert.NotNull(jwtToken);
        Assert.Equal("TestIssuer", jwtToken.Issuer); // <-- test the secret.
        Assert.Equal("TestAudience", jwtToken.Audiences.First());
        Assert.True(jwtToken.ValidTo > DateTime.UtcNow);
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















