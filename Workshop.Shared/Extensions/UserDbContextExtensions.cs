using Workshop.Shared.Data;
using Workshop.Shared.Models;

namespace Workshop.Shared.Extensions;

public static class UserDbContextExtensions
{
    public static async Task AddTestData(this UserDbContext context)
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

        await context.AddRangeAsync(users);

        await context.SaveChangesAsync();
    }
}