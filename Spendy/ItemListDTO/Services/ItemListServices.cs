using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Spendy.Shared.Data;
using Spendy.Shared.Helpers;
using Spendy.Shared.Models;

namespace ItemListDTO.Services;

public interface IItemListService
{
    public List<Item> GetItems();
    public List<ProductIdItem> GetItemByProductId(int productId);
}


public class ItemListService: IItemListService
{

    private readonly SpendyContext _context;
    // private static List<Item> ItemList { get; }

    public ItemListService(SpendyContext context)
    {
        _context = context;
    }
    
    public List<Item> GetItems()
    {   
        // raw sql query instead of linq as easy to understand
        // string sql;
        //
        // var assembly = Assembly.GetExecutingAssembly();
        // var resourceName = "Spendy.Shared.Models.GetItems.sql";
        //
        // using (var stream = assembly.GetManifestResourceStream(resourceName))
        // using (var reader = new StreamReader(stream))
        // {
	       //  sql = reader.ReadToEnd();
        // }

        var sql = ResourceReader.ReadEmbeddedResource("Spendy.Shared.Models.GetItems.sql");
        
        var items = _context.Items
            .FromSqlRaw(sql).ToList();
        
        return items;
    }

    public List<ProductIdItem> GetItemByProductId(int productId)
    {   
        var sql = ResourceReader.ReadEmbeddedResource("Spendy.Shared.Models.GetItemsByProductId.sql");
        
        var items = _context.ProductIdItems
            .FromSqlRaw(sql, new MySqlParameter("productId", productId))
            .ToList();
        return items;
    }
}