using Workshop.Shared.Models;

namespace Workshop.Shared.Services;

public class UserManager : IUserManager
{
    public Task<User> Get(string login)
    {
        throw new NotImplementedException();
    }
}