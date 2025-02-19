using Microsoft.EntityFrameworkCore;
using Workshop.Shared.Models;

namespace Workshop.Shared.Data;

public class UserDbContext : DbContext
{
    public UserDbContext() : base()
    {
    }

    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<User>()
            .HasKey(x => x.Id);

        modelBuilder.Entity<RefreshToken>()
            .HasKey(x => x.UserId);
        
        modelBuilder.Entity<RefreshToken>()
            .HasIndex(x => x.Value)
            .IsUnique();
        
        modelBuilder.Entity<RefreshToken>()
            .Property(p => p.Value)
            .IsRequired();
    }

    public virtual DbSet<User> Users { get; set; }
    
    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
}