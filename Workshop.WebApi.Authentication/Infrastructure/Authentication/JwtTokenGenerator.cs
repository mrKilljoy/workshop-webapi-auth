using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Workshop.WebApi.Authentication.Infrastructure.Configuration;

namespace Workshop.WebApi.Authentication.Infrastructure.Authentication;

public class JwtTokenGenerator : ITokenGenerator
{
    private readonly IOptions<SecurityOptions> _options;

    public JwtTokenGenerator(IOptions<SecurityOptions> options)
    {
        _options = options;
    }
    
    public string Generate(string login)
    {
        ArgumentNullException.ThrowIfNull(login, nameof(login));
        
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, login),
            new Claim(ClaimTypes.Name, login),
            new Claim(Constants.Authentication.Claims.TestClaimName, Constants.Authentication.Claims.TestClaimValue)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GetEncryptionKey()));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: Constants.Authentication.JwtIssuer,
            audience: Constants.Authentication.JwtAudience,
            claims: claims,
            expires: DateTime.Now.AddSeconds(GetTokenLifetime()),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GetEncryptionKey() => _options.Value?.EncryptionKey;

    private int GetTokenLifetime() => _options.Value?.AccessTokenLifetimeSeconds ?? default;
}