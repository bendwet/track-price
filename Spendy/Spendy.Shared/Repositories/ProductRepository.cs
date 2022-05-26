using Spendy.Shared.Data;
using Spendy.Shared.Models;

namespace Spendy.Shared.Repositories;

public class ProductRepository
{
    private readonly SpendyContext _context;

    public ProductRepository(SpendyContext context)
    {
        _context = context;
    }
    // Return product with given name or return null is product does not exist
    public Product? GetByName(string productName)
    {
        var product = _context.Products
            .FirstOrDefault(p => p.ProductName == productName);
        return product;
    }
    // save a new product
    public void Save(Product product)
    {
        Console.WriteLine("Save Product");
        _context.Add(product);
    }
    // if product exists already, update it
    public void Update(Product product)
    {
        Console.WriteLine();
        _context.Update(product);
    }
    // delete a product
    public void Delete()
    {
        Console.WriteLine("Delete Product");
    }
}