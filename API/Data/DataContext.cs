using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContext : IdentityDbContext<AppUser, AppRole, int, IdentityUserClaim<int>,
 AppUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    // Entity Framework then creates a table with the name Users
    // public DbSet<AppUser> Users { get; set; } // no longer needed due to using ASP.NET Core Identity
    public DbSet<UserConnectionRequest> ConnectionRequests { get; set; }
    public DbSet<Message> Messages { get; set; } // table will be called Messages

    // will override the OnModelCreating method from DbContext class, which DataContext is derived from
    protected override void OnModelCreating(ModelBuilder builder)
    {
        // this uses the method inside the DbContext base class and passes it the builder object
        base.OnModelCreating(builder);

        builder.Entity<AppUser>()
            .HasMany(ur => ur.UserRoles)
            .WithOne(u => u.User)
            .HasForeignKey(ur => ur.UserId)
            .IsRequired();

        builder.Entity<AppRole>()
            .HasMany(ur => ur.UserRoles)
            .WithOne(u => u.Role)
            .HasForeignKey(ur => ur.RoleId)
            .IsRequired();            

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

        builder.Entity<Message>()
            .HasOne(u => u.Recipient)
            .WithMany(m => m.MessagesReceived)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Message>()
            .HasOne(u => u.Sender)
            .WithMany(m => m.MessagesSent)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
