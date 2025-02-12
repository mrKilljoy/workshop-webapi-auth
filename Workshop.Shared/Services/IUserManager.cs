using Workshop.Shared.Models;

namespace Workshop.Shared.Services;

public interface IUserManager
{
    Task<User> Get(string login);
}