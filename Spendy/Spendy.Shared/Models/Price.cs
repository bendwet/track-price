using System.ComponentModel.DataAnnotations.Schema;

#pragma warning disable CS8618

namespace Spendy.Shared.Models;

[Table("prices")]
public record Price
{   
    [Column("price_id")]
    public int PriceId { get; init; }
    [Column("product_id")]
    public int ProductId { get; init; }
    [Column("store_id")]
    public int StoreId { get; init; }
    [Column("price_date")]
    public DateTime PriceDate { get; init; }
    [Column("price")]
    public double OriginalPrice { get; init; }
    [Column("is_onsale")]
    public bool IsOnSale { get; init; }
    [Column("price_sale")]
    public double SalePrice { get; init; }
    [Column("is_available")]
    public bool IsAvailable { get; init; }
    [Column("price_quantity")]
    public string PriceQuantity { get; init; }
    public Product Product { get; init; }
    public Store Store { get; init; }

}
