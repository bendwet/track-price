namespace SpendyBackend.Models;

public class Price
{
    public int PriceId { get; set; }
    public int ProductId { get; set; }
    public int StoreId { get; set; }
    public DateTime PriceDate { get; set; }
    public double PriceOriginal { get; set; }
    public bool IsOnSale { get; set; }
    public double PriceSale { get; set; }
    public bool IsAvailable { get; set; }
    
    public virtual Product Product { get; set; }
    public virtual Store Store { get; set; }
    
}
