using Workshop.WebApi.Cookie.Models;

namespace Workshop.WebApi.Cookie.Infrastructure.Authentication;

public interface ITokenGenerator
{
    string Generate(CredentialsModel model);
}