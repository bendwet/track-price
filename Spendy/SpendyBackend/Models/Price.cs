namespace SpendyBackend.Models;

public class Price
{
    public int PriceId { get; set; }
    public int ProductId { get; set; }
    public int StoreId { get; set; }
    public DateTime PriceDate { get; set; }
    public decimal PriceOriginal { get; set; }
    public bool IsOnSale { get; set; }
    public decimal PriceSale { get; set; }
    public bool IsAvailable { get; set; }
}
