using Microsoft.EntityFrameworkCore;

namespace Company.Product.WebApi.Data;

public sealed class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Entity> Tables { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
}
