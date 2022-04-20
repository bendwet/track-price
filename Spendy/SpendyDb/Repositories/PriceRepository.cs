namespace SpendyDb.Repositories;

public interface IPriceRepository
{
    public void Save();
    public void Delete();
}

public class PriceRepository : IPriceRepository
{
    public void Save()
    {
        Console.WriteLine("Save Price");
    }

    public void Delete()
    {
        Console.WriteLine("Delete Price");
    }
}