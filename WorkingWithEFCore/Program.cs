using Packt.Shared;

NorthWind db = new();
WriteLine(db.Database.ProviderName);