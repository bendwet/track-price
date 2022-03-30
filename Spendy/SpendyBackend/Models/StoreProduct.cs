using System.ComponentModel.DataAnnotations.Schema;

namespace SpendyBackend.Models;

[Table("store_products")]
public class StoreProduct
{   
    [Column("product_id")]
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