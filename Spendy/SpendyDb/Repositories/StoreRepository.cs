namespace SpendyDb.Repositories;

public class StoreRepository : ISpendyRepository
{
    public void Save()
    {
        Console.WriteLine("Save Store");
    }

    public void Delete()
    {
        Console.WriteLine("Delete Store");
    }
        
}