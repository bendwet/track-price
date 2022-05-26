using System.ComponentModel.DataAnnotations.Schema;

#pragma warning disable CS8618

namespace Spendy.Shared.Models;

[Table("store_products")]
public record StoreProduct
{   
    [Column("store_product_id")]
    public int StoreProductId { get; init; }
    [Column("store_id")]
    public int StoreId { get; init; }
    [Column("product_id")]
    public int ProductId { get; init; }
    [Column("store_product_code")]
    public string StoreProductCode { get; init; }
    
    public Store Store { get; init; }
    public Product Product { get; init; }
    
}