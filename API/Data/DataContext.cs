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
}
