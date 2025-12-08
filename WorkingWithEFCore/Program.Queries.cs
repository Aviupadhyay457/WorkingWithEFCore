using Microsoft.EntityFrameworkCore;
using Packt.Shared;
partial class Program {
    static void QueryingCategories()
    {
        using (NorthWind db = new())
        {
            WriteLine("Products in each Category");

            IQueryable<Category> categories = db.Categories.Include(c=>c.Products);

            if(categories is null || !categories.Any())
            {
                WriteLine("no records present in categories table");
            }
            foreach(Category c in categories)
            {
                WriteLine($"{c.CategoryName} has {c.Products.Count} products.");
            }
        }
    }

}
