using PuppeteerSharp;

#pragma warning disable CS8618
namespace PriceRetriever.Interfaces;

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
    public Task<PriceModel> RetrievePrice(string storeProductCode);
}
