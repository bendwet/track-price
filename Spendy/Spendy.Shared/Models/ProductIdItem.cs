using System.ComponentModel.DataAnnotations.Schema;

namespace Spendy.Shared.Models;

public class ProductIdItem
{
    [Column("product_id")]
    public int ProductId { get; set; }
    [Column("store_name")]
    public string StoreName { get; set; }
    [Column("price_sale")]
    public double? SalePrice { get; set; }
    [Column("price_date")]
    public DateTime? PriceDate { get; set; }
    [Column("is_onsale")]
    public bool? IsOnSale { get; set; }
    [Column("is_available")]
    public bool? IsAvailable { get; set; }
    [Column("product_name")]
    public string ProductName { get; set; }
    [Column("price_quantity")]
    public string? PriceQuantity { get; set; }

}