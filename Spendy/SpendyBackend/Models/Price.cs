using System.ComponentModel.DataAnnotations.Schema;

namespace SpendyBackend.Models;

[Table("prices")]
public class Price
{
    [Column("price_id")]
    public int PriceId { get; set; }
    [Column("product_id")]
    public int ProductId { get; set; }
    [Column("store_id")]
    public int StoreId { get; set; }
    [Column("price_date")]
    public DateTime PriceDate { get; set; }
    [Column("price")]
    public double PriceOriginal { get; set; }
    [Column("is_onsale")]
    public bool IsOnSale { get; set; }
    [Column("price_sale")]
    public double PriceSale { get; set; }
    [Column("is_available")]
    public bool IsAvailable { get; set; }
    
    public Product Product { get; set; }
    public Store Store { get; set; }
    
}
