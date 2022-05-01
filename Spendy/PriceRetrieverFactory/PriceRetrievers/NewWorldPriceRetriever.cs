using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using PriceRetrieverFactory.PriceRetrievers.Constants;
using PriceRetrieverFactory.Interfaces;

namespace PriceRetrieverFactory.PriceRetrievers;

public class NewWorldPriceRetriever: IPriceRetriever
{
    private readonly HttpClient _client;

    public NewWorldPriceRetriever(HttpClient client)
    {
        _client = client;
    }
    
    private class NewWorldPriceResponseModel
    {
        
    }

    public Task<PriceModel> RetrievePrice(string storeProductCode)
    {
        var url = $"https://www.newworld.co.nz/shop/product/{storeProductCode}_ea_000nw";
        
        // add headers
        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8"));
        _client.DefaultRequestHeaders.Add("User-Agent",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:99.0) Gecko/20100101 Firefox/99.0");
        // client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
        _client.DefaultRequestHeaders.Add("cookie", NewWorldCookie.Cookie);
        
    }

}