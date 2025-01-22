namespace Ally.Domain.Dtos;

public class LoginDto
{
    // will contain the token, its expiresIn value
    public string Token { get; set; }
    public DateTime TokenExpiresIn { get; set; }
}
