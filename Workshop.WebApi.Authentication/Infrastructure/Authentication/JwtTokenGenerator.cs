using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Workshop.WebApi.Authentication.Infrastructure.Configuration;
using Workshop.WebApi.Authentication.Infrastructure.Exceptions;
using Workshop.WebApi.Authentication.Models;

namespace Workshop.WebApi.Authentication.Infrastructure.Authentication;

public class JwtTokenGenerator : ITokenGenerator
{
    private const int TokenExpirationTimeSeconds = 60 * 10;  // ideally, it should be a configurable parameter
    
    private readonly IConfiguration _configuration;

    public JwtTokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public string Generate(string login)
    {
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
            expires: DateTime.Now.AddSeconds(TokenExpirationTimeSeconds),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GetEncryptionKey()
    {
        return _configuration?
            .GetSection(Constants.Configuration.SecuritySection)?
            .Get<SecurityOptions>()?.EncryptionKey
               ?? throw new MissingEncryptionKeyException();
    }
}