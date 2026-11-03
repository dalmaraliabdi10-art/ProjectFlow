using Microsoft.EntityFrameworkCore;

namespace ProjectFlow.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // DbSet<T> properties are added once entities exist (Fas 1).
}
