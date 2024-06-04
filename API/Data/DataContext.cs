using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    // Entity Framework then creates a table with the name Users
    public DbSet<AppUser> Users { get; set; }
    public DbSet<UserConnectionRequest> ConnectionRequests { get; set; }

    // will override the OnModelCreating method from DbContext class, which DataContext is derived from
    protected override void OnModelCreating(ModelBuilder builder)
    {
        // this uses the method inside the DbContext base class and passes it the builder object
        base.OnModelCreating(builder);

        builder.Entity<UserConnectionRequest>()
            .HasKey(k => new { k.SourceUserId, k.TargetUserId }); // this represents the primary key used in the ConnectionRequests table

        builder.Entity<UserConnectionRequest>()
            .HasOne(s => s.SourceUser)
            .WithMany(l => l.ConnectionRequestedFromUsers)
            .HasForeignKey(s => s.SourceUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<UserConnectionRequest>()
            .HasOne(s => s.TargetUser)
            .WithMany(l => l.ConnectionRequestedByUsers)
            .HasForeignKey(s => s.TargetUserId)
            .OnDelete(DeleteBehavior.Cascade); // if using SQL Server, you must use .NoAction instead
    }
}
