using SpendyDb.Data;
using SpendyDb.Models;

namespace SpendyDb.Repositories;

public interface IStoreProductRepository
{
    public void Save();
    public void Delete();
    // public StoreProduct RetrieveByStoreProductCode(string storeProductCode, int storeId);
    public List<StoreProduct> RetrieveByStoreId(int storeId);
}

public class StoreProductRepository : IStoreProductRepository
{   
    private readonly SpendyContext _context;
    private StoreProductRepository(SpendyContext context)
    {
        _context = context;
    }

    public List<StoreProduct> RetrieveByStoreId(int storeId)
    {
        var storeProductCodes = _context.StoreProducts
            .Where(s => s.StoreId == storeId).ToList();

        return storeProductCodes;
    }

    // get store product by store product code (and store id as may have multiple entries with same code)
    // public StoreProduct RetrieveByStoreProductCode(string storeProductCode, int storeId)
    // {
    //    var storeProduct = _context.StoreProducts
    //         .Single(s => s.StoreProductCode == storeProductCode && s.StoreId == storeId);
    //    return storeProduct;
    // }
    public void Save()
    {
        Console.WriteLine("Save Store Product");
    }
    public void Delete()
    {
        Console.WriteLine("Delete Store Product");
    }
}