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
    public List<LowestPriceDateItem> GetLowestPriceItemPerDate(int productId);
}


public class ItemListService: IItemListService
{

    private readonly SpendyContext _context;

    public ItemListService(SpendyContext context)
    {
        _context = context;
    }
    
    // get all items with latest date
    public List<Item> GetItems()
    {
        var sql = ResourceReader.ReadEmbeddedResource("Spendy.Shared.Models.GetItems.sql");
        
        var items = _context.Items
            .FromSqlRaw(sql).ToList();
        
        return items;
    }
    
    // get items by product id with latest date
    public List<ProductIdItem> GetItemByProductId(int productId)
    {
        var sql = ResourceReader.ReadEmbeddedResource("Spendy.Shared.Models.GetItemsByProductId.sql");
        
        var items = _context.ProductIdItems
            .FromSqlRaw(sql, new MySqlParameter("productId", productId))
            .ToList();
        return items;
    }
    
    // get lowest price of item for each date by product id
    public List<LowestPriceDateItem> GetLowestPriceItemPerDate(int productId)
    {
        var items = _context.Prices
            .Select(p => new LowestPriceDateItem
            {
                ProductId = p.ProductId,
                SalePrice = p.SalePrice,
                IsAvailable = p.IsAvailable,
                PriceDate = p.PriceDate
            })
            .Where(p => p.ProductId == productId 
                        && p.IsAvailable == true)
            .ToList();

        return items;
    }
}