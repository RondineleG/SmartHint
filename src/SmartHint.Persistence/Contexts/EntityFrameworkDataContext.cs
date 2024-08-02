namespace SmartHint.Persistence.Contexts;

public sealed class EntityFrameworkDataContext(DbContextOptions<EntityFrameworkDataContext> options) : DbContext(options)
{
    public DbSet<Comprador> Compradores { get; set; }
        
}
