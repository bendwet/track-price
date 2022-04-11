using SpendyDb.Data;
using SpendyDb.Models;

namespace SpendyDb.Repositories;

public class StoreRepository
{
    private readonly SpendyContext _context;

    public StoreRepository(SpendyContext context)
    {
        _context = context;
    }
    // Get a certain store by its store name
    public Store GetByStoreName(string storeName)
    {
        var store = _context.Stores
            .Single(s => s.StoreName == storeName);
        return store;
    }
    // Add a new store
    public void Save(string storeName)
    {
        Console.WriteLine("Save Store");
        _context.Add(new Store{StoreName = storeName});
    }
    // Delete a store
    public void Delete()
    {
        Console.WriteLine("Delete Store");
    }
    
}