using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Packt.Shared;

public class  NorthWind:DbContext
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string path = Path.Combine(Environment.CurrentDirectory, "Northwind.db");
        string connection = $"Filename={path}";
        optionsBuilder.UseSqlite(connection);
        optionsBuilder.UseLazyLoadingProxies();

        optionsBuilder.LogTo(WriteLine, new[] { RelationalEventId.CommandExecuting }).EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>()
            .Property(Category => Category.CategoryName)
            .IsRequired()
            .HasMaxLength(15);

        if (Database.ProviderName?.Contains("Sqlite") == true)
        {
            modelBuilder.Entity<Product>()
                .Property(Product => Product.Cost)
                .HasConversion<double>();
        }

        modelBuilder.Entity<Product>()
            .HasQueryFilter(p => !p.Discontinued);
    }


}