using Microsoft.EntityFrameworkCore;
using Workshop.Shared.Models;

namespace Workshop.Shared.Data;

public class UserDbContext : DbContext
{
    public virtual DbSet<User> Users { get; set; }

    public async Task AddTestData()
    {
        var users = new List<User>()
        {
            new User()
            {
                Id = Guid.NewGuid(),
                Login = "test1@test",
                Password = "123456"
            },
            new User()
            {
                Id = Guid.NewGuid(),
                Login = "test2@test",
                Password = "7890"
            },
            new User()
            {
                Id = Guid.NewGuid(),
                Login = "test3@test",
                Password = "543210"
            }
        };

        await AddRangeAsync(users);

        await SaveChangesAsync();
    }
}