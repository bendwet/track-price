namespace SpendyBackend.Models;

public class StoreProduct
{
    public int StoreProductId { get; set; }
    public int StoreId { get; set; }
    public int ProductId { get; set; }
    public string StoreProductCode { get; set; }
    
    public virtual Store Store { get; set; }
    public virtual Product Product { get; set; }
    
}