using SpendyDb.Data;

namespace SpendyDb.Repositories;

public interface IPriceRepository
{
    public void Save();
    public void Delete();
}

public class PriceRepository : IPriceRepository
{
    private readonly SpendyContext _context;

    private PriceRepository(SpendyContext context)
    {
        _context = context;
    }
    public void Save()
    {
        _context.Prices.Single();
        Console.WriteLine("Save Price");
    }

    public void Delete()
    {
        Console.WriteLine("Delete Price");
    }
}