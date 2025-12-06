using Microsoft.EntityFrameworkCore;

namespace Packt.Shared;

public class  NorthWind:DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string path = Path.Combine(Environment.CurrentDirectory, "Northwind.db");
        string connection = $"Filename={path}";
        optionsBuilder.UseSqlite(connection);

    }

}