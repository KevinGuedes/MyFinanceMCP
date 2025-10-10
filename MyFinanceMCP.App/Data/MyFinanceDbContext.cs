using Microsoft.EntityFrameworkCore;
using MyFinanceMCP.App.Models;

namespace MyFinanceMCP.App.Data;

/// <summary>
/// Represents the Entity Framework Core database context for the MyFinanceMCP application.
/// </summary>
public class MyFinanceDbContext(DbContextOptions<MyFinanceDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Gets or sets the collection of <see cref="Models.Transfer"/> entities in the database.
    /// </summary>
    public DbSet<Transfer> Transfers { get; set; } = null!;

    /// <summary>
    /// Configures the EF Core model for the application entities.
    /// </summary>
    /// <param name="modelBuilder">The builder used to construct the model for the context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Transfer>(eb =>
        {
            eb.HasKey(e => e.Id);
            eb.Property(e => e.Value).IsRequired().HasColumnType("decimal(18,2)");
            eb.Property(e => e.Description).IsRequired();
            eb.Property(e => e.Date).IsRequired();
            eb.Property(e => e.Type).HasConversion<string>().IsRequired();
        });
    }
}
