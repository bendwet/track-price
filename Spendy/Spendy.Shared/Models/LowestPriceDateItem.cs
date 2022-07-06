using System.ComponentModel.DataAnnotations.Schema;

namespace Spendy.Shared.Models;

public class LowestPriceDateItem
{   
    [Column("product_id")]
    public int ProductId { get; set; }
    [Column("price_sale")]
    public double SalePrice { get; set; }
    [Column("is_available")]
    public bool IsAvailable { get; set; }
    [Column("price_date")]
    public DateTime PriceDate { get; set; }
}