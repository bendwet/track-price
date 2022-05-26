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
        Console.WriteLine(priceRecord.SalePrice);
        _context.Add(priceRecord);
        _context.SaveChanges();
    }

    public void Delete()
    {
        Console.WriteLine("Delete Price");
    }
}