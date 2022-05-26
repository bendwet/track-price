using Spendy.Shared.Data;
using Spendy.Shared.Models;

namespace Spendy.Shared.Repositories;

public interface IStoreRepository
{
    public List<Store> GetAllStores();
    public Store GetByName(string storeName);
    public void Save(string storeName);
    public void Delete();
}

public class StoreRepository: IStoreRepository
{
    private readonly SpendyContext _context;

    public List<Store> GetAllStores()
    {   
        // retrieve all stores from database and return as a list
        var stores = _context.Stores.ToList();
        return stores;
    }
    
    public StoreRepository(SpendyContext context)
    {
        _context = context;
    }
    // Get a certain store by its store name
    public Store GetByName(string? storeName)
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