using Microsoft.EntityFrameworkCore;
using Workshop.Shared.Data;
using Workshop.Shared.Models;

namespace Workshop.Shared.Services;

public class UserManager : IUserManager
{
    private readonly UserDbContext _dbContext;

    public UserManager(UserDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public Task<User> Get(string login)
    {
        return _dbContext.Users.FirstOrDefaultAsync(x => x.Login.Equals(login));
    }

    public Task<User> Get(string login, string password)
    {
        return _dbContext.Users.FirstOrDefaultAsync(x =>
            x.Login.Equals(login) &&
            x.Password.Equals(password));
    }
}