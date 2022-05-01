#pragma warning disable CS8618
namespace PriceRetrieverFactory.Interfaces;

public class PriceModel
{
    public double OriginalPrice { get; set; }
    public double SalePrice { get; set; }
    public DateTime PriceDate { get; set; }
    public bool IsOnSale { get; set; }
    public bool IsAvailable { get; set; }
    public string PriceQuantity { get; set; }
    
}

public interface IPriceRetriever
{
    Task<PriceModel> RetrievePrice(string storeProductCode);
}