using PuppeteerSharp;
using SpendyDb.Models;

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

public interface IApiPriceRetriever
{
    public Task<PriceModel> RetrievePrice(string storeProductCode);
}

public interface IWebScrapePriceRetriever
{
    public Task<PriceModel> RetrievePrice(BrowserFetcher browserFetcher, string storeProductCode);
}