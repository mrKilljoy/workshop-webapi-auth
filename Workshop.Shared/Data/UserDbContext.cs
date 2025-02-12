using Microsoft.EntityFrameworkCore;
using Workshop.Shared.Models;

namespace Workshop.Shared.Data;

public class UserDbContext : DbContext
{
    public virtual DbSet<User> Users { get; set; }

    public void AddTestData()
    {
        // todo: add some users
    }
}