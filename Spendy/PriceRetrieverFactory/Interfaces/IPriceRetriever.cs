namespace PriceRetrieverFactory;

public class PriceModel
{
    public decimal OriginalPrice { get; set; }
    public decimal SalePrice { get; set; }
    public DateTime PriceDate { get; set; }
    public bool IsOnSale { get; set; }
    public bool IsAvailable { get; set; }
    public string PriceQuantity { get; set; }
    
}

public interface IPriceRetriever
{
    Task<PriceModel> RetrievePrice(string storeProductCode);
}