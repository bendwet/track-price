using SpendyDb.Data;
using SpendyDb.Models;

namespace SpendyDb.Repositories;

public interface IPriceRepository
{
    public void Save(Price priceRecord);
    public void Delete();
}

public class PriceRepository : IPriceRepository
{
    private readonly SpendyContext _context;

    private PriceRepository(SpendyContext context)
    {
        _context = context;
    }
    public void Save(Price priceRecord)
    {
        Console.WriteLine("Save Price");
        _context.Add(priceRecord);
    }

    public void Delete()
    {
        Console.WriteLine("Delete Price");
    }
}