using Microsoft.EntityFrameworkCore;
using NovibetApi.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Country> Countries { get; set; }
    public DbSet<IPAddress> IPAddresses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure entity properties and relationships
        // different names classes and tables
        // connectionstring

        modelBuilder.Entity<Country>(entity =>
        {
            entity.ToTable("Countries");
        });

        modelBuilder.Entity<IPAddress>(entity =>
        {
            entity.ToTable("IPAddresses");
        });
    }
}