using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Workshop.WebApi.Cookie.Infrastructure.Configuration;
using Workshop.WebApi.Cookie.Infrastructure.Exceptions;
using Workshop.WebApi.Cookie.Models;

namespace Workshop.WebApi.Cookie.Infrastructure.Authentication;

public class JwtTokenGenerator : ITokenGenerator
{
    private const int TokenExpirationTimeSeconds = 60 * 10;  // ideally, it should be a configurable parameter
    
    private readonly IConfiguration _configuration;

    public JwtTokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public string Generate(CredentialsModel model)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, model.Login),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, model.Login)
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
            .Get<Security>()?.EncryptionKey
               ?? throw new MissingEncryptionKeyException();
    }
}