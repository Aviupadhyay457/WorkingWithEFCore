using System.Runtime.InteropServices.Marshalling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Packt.Shared;
using Microsoft.EntityFrameworkCore.ChangeTracking;
partial class Program {
    static void QueryingCategories()
    {
        using (NorthWind db = new())
        {
            WriteLine("Products in each Category");

            IQueryable<Category>? categories
            = db?.Categories;
            //?.Include(c=>c.Products);

            db.ChangeTracker.LazyLoadingEnabled = false;
            if(categories is null || !categories.Any())
            {
                WriteLine("no records present in categories table");
            }
            WriteLine(categories.ToQueryString());
            foreach(Category c in categories)
            {
                WriteLine($"{c.CategoryName} has {c.Products.Count} products.");
            }
        }
    }

    static void FilterIncludes()
    {

        using NorthWind db = new();
        Write("Enter the lower limit of items:");
        string ?input = ReadLine();
        int stock;
        bool success = int.TryParse(input, out stock);
        if (!success) {
            WriteLine("invalid numeric value");
            return;
        }

        IQueryable<Category> categories = db.Categories.Include
            (c => c.Products.Where(p=>p.Stock>=stock));

        WriteLine($"ToQueryString: {categories.ToQueryString()}");

        foreach (Category c in categories)
        {
            WriteLine($"{c.CategoryName} has {c.Products.Count} which has over {stock} items.");
            foreach(Product p in c.Products)
            {
                WriteLine($"             {p.ProductName} has {p.Stock} items");
            }
        }
    }

    static void QueryingProducts()
    {
        using(NorthWind db = new())
        {
            WriteLine("Products that cost more than a price, highest at top.");
            Write("Minimum Price should be:");
            string? input = ReadLine();
            decimal price;
            bool success = decimal.TryParse(input, out price);
            if(!success)
            {
                WriteLine("invalid Price");
                return;
            }
            IQueryable<Product>? products= db.Products?.TagWith("Products filtered by price and sorted.").Where(p=>p.Cost>=price).OrderByDescending(p=>p.Cost);

            
            if(products is null || (!products.Any()))
            {
                WriteLine("no products found");
                return;
            }
            //WriteLine($"ToQueryString: {products.ToQueryString()}");

            foreach (Product p in products)
            {
                WriteLine($"{p.ProductName} costs {p.Cost} and has {p.Stock} units in stocks");
            }

        }
    }

    static void QueryingWithLike()
    {
        using (NorthWind db = new())
        {
            WriteLine("Pattern Matching with like%");
            Write("search for a product:");
            string? input = ReadLine();
            if (String.IsNullOrWhiteSpace(input))
            {
                IQueryable<Product> products = db.Products;
                WriteLine("returning every product");
                foreach (Product p in products)
                {
                    
                    WriteLine(p.ProductName);
                }
         
            }
            else
            {
                IQueryable<Product>? products = db.Products?.Where(p => EF.Functions.Like(p.ProductName, $"%{input}%"));
                if(products is null || !products.Any())
                {
                    WriteLine("no such products");
                    return;
                }
                foreach (Product p in products)
                {
                    WriteLine("{0} has {1} units in stock. Discontinued? {2}",
                    p.ProductName, p.Stock, p.Discontinued);
                }
            }

            //int? countRows = db.Products?.Count();
            //Product? pr = db?.Products.FirstOrDefault(pr => pr.ProductId == (int)(EF.Functions.Random() * countRows));
            //WriteLine($"The random product is {pr.ProductName}");

        }
    }

}
