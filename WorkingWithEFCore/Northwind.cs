using Microsoft.EntityFrameworkCore;

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

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>()
            .Property(Category => Category.CategoryName)
            .IsRequired()
            .HasMaxLength(15);

        if (Database.ProviderName?.Contains("Sqlite") == false)
        {
            modelBuilder.Entity<Product>()
                .Property(Product => Product.Cost)
                .HasConversion<double>();
        }
    }


}