using SpendyDb.Models;
using SpendyDb.Data;

namespace SpendyDb;

class Program
{
    static void Main()
    {
        using var db = new SpendyContext();
        // Note: This sample requires the database to be created before running.
        Console.WriteLine($"Database path: {db.DbPath}.");
                
        // Create
        Console.WriteLine("Inserting a new store");
        db.Add(new Store { StoreName = "Countdown"});
        db.SaveChanges();

        // Read
        Console.WriteLine("Querying for a store");
        var store = db.Stores
            .First();
        Console.WriteLine(store);
        
        
        // Update
        Console.WriteLine("Updating");
        store.StoreName = "Paknsave";
        db.SaveChanges();
    }
}