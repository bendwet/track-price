using System.ComponentModel.DataAnnotations.Schema;

#pragma warning disable CS8618

namespace Spendy.Shared.Models;

[Table("store_products")]
public class StoreProduct
{   
    [Column("store_product_id")]
    public int StoreProductId { get; set; }
    [Column("store_id")]
    public int StoreId { get; set; }
    [Column("product_id")]
    public int ProductId { get; set; }
    [Column("store_product_code")]
    public string StoreProductCode { get; set; }
    
    public Store Store { get; set; }
    public Product Product { get; set; }
    
}