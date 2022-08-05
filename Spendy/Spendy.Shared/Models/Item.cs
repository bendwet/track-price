using System.ComponentModel.DataAnnotations.Schema;

#pragma warning disable CS8618
namespace Spendy.Shared.Models;

public class Item
{
    [Column("price_sale")]
    public double? SalePrice { get; set; }
    [Column("price_date")]
    public DateTime? PriceDate { get; set; }
    [Column("is_onsale")]
    public bool? IsOnSale { get; set; }
    [Column("is_available")]
    public bool? IsAvailable { get; set; }
    [Column("product_id")]
    public int ProductId { get; set; }
    [Column("product_name")]
    public string ProductName { get; set; }
    [Column("price_quantity")]
    public string? PriceQuantity { get; set; }
    [Column("product_is_out_of_stock")]
    public bool ProductOutOfStock { get; set; }
}