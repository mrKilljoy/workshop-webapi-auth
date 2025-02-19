using Workshop.WebApi.Authentication.Models;

namespace Workshop.WebApi.Authentication.Infrastructure.Authentication;

public interface ITokenGenerator
{
    string Generate(string login);
}