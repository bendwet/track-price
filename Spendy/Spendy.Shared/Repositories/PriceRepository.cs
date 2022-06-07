using Spendy.Shared.Data;
using Spendy.Shared.Models;

namespace Spendy.Shared.Repositories;

public interface IPriceRepository
{
    public void Save(Price priceRecord);
    public void Delete();
}

public class PriceRepository : IPriceRepository
{
    private readonly SpendyContext _context;

    public PriceRepository(SpendyContext context)
    {
        _context = context;
    }
    public void Save(Price priceRecord)
    {
        Console.WriteLine("Save Price");
        // check if price already exists
        var priceExists = _context.Prices.FirstOrDefault(p => p.ProductId == priceRecord.ProductId &&
                                                              p.StoreId == priceRecord.StoreId &&
                                                              p.PriceDate == priceRecord.PriceDate);

        if (priceExists == null)
        {
            try
            {
                Console.WriteLine($"Product Id: {priceRecord.ProductId}");
                _context.Add(priceRecord);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        else
        {   
            Console.WriteLine($"Product Id: {priceRecord.ProductId}");
            priceExists.OriginalPrice = priceRecord.OriginalPrice;
            priceExists.SalePrice = priceRecord.SalePrice;
            priceExists.IsOnSale = priceRecord.IsOnSale;
            priceExists.IsAvailable = priceRecord.IsAvailable;
            priceExists.PriceQuantity = priceRecord.PriceQuantity;
            _context.SaveChanges();
        }
    }

    public void Delete()
    {
        Console.WriteLine("Delete Price");
    }
}