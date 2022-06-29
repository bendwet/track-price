using Microsoft.EntityFrameworkCore;
using Spendy.Shared.Data;
using Spendy.Shared.Models;


namespace ItemListDTO.Services;

public interface IItemListService
{
    public List<Item> GetItems();
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
        var items = _context.Items
            .FromSqlRaw(@"
            WITH
                last_date as (
                    SELECT 
                        MAX(price_date) as max_date
                    FROM
                        pricedb.prices    
                )
                , product_out_of_stock as (
                    SELECT
                        p.product_id
                    FROM
                        pricedb.prices p
                    WHERE
                        p.price_date = (SELECT MAX(p.price_date) FROM prices p)
                    GROUP BY
                        product_id
                    HAVING
                        MAX(p.price_sale) = 0
                )
                , lowest_price_per_product_day_ranked as (
                    SELECT
                        p.price_id
                        , p.product_id
                        , products.product_name
                        , p.price_sale
                        , p.price_quantity
                        , p.is_available
                        , p.is_onsale
                        , p.price_date
                        , RANK() OVER (PARTITION BY p.product_id ORDER BY p.price_sale ASC) as price_rank
                    FROM
                        pricedb.prices p
                        INNER JOIN last_date ld ON ld.max_date = p.price_date
                        INNER JOIN products ON p.product_id = products.product_id
                    WHERE
                        p.is_available = true
                )
                , lowest_price_per_product_day as (
                    SELECT 
                        *
                    FROM
                        lowest_price_per_product_day_ranked
                    WHERE
                        price_rank = 1
                    GROUP BY
                        product_id
                )
                SELECT 
                    p.product_id
                    , p.product_name
                    , lpd.price_sale
                    , lpd.price_quantity
                    , lpd.is_available
                    , lpd.is_onsale
                    , lpd.price_date
                    , case when pos.product_id is null then 0 else 1 end as product_is_out_of_stock
                FROM
                    products p
                    LEFT JOIN lowest_price_per_product_day lpd ON lpd.product_id = p.product_id
                    LEFT JOIN product_out_of_stock pos ON pos.product_id = p.product_id")
            .ToList();

        // var items1 = _context.Items
        //     .FromSqlRaw(@"
        //     SELECT 
        //         prices.price_date
        //         , MIN(prices.price_sale) as lowest_product_price
        //         , prices.is_onsale
        //         , prices.price_quantity
        //         , prices.is_available
        //         , products.product_id
        //         , products.product_name 
        //     FROM 
        //         pricedb.prices 
        //         INNER JOIN products ON prices.product_id = products.product_id
        //     WHERE 
        //         prices.price_date = (SELECT max(prices.price_date) FROM pricedb.prices)
        //     GROUP BY 
        //         product_id")
        //     .ToList();
        
        return items;
    }
    
    // public void GetItems()
    // {
    //     var maxPrice = _context.Prices.Max(x => x.PriceDate);
    //     var prices = _context
    //         .Prices
    //         .Include(x => x.Product)
    //         .Include(x => x.Store)
    //         .Where(x => x.PriceDate == maxPrice)
    //         .Select(x => new {x.Store.StoreName, x.Product.ProductName, x.SalePrice})
    //         .GroupBy(x => new {x.StoreName, x.ProductName})
    //         .ToList();
    //
    //     foreach (var groupedPrice in prices)
    //     {
    //         var minPrice = groupedPrice.Min(x => x.SalePrice);
    //         var minPriceStore = groupedPrice.Where(x => x.SalePrice == minPrice).First();
    //         
    //         Console.WriteLine($"Store={minPriceStore.StoreName}, product={minPriceStore.ProductName}, price={minPriceStore.SalePrice}");
    //     }
    //
    //
    // }
    
}