namespace SpendyBackend.Models;

public class StoreProduct
{
    public int StoreProductId { get; set; }
    public int StoreId { get; set; }
    public int ProductId { get; set; }
    # nullable disable
    public string StoreProductCode { get; set; }
    
    public Store Store { get; set; }
    public Product Product { get; set; }
    
}