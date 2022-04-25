using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PriceRetrieverFactory.PriceRetrievers;

public class CountdownPriceRetriever : IPriceRetriever
{
    private readonly HttpClient _client;

    public CountdownPriceRetriever(HttpClient client)
    {
        _client = client;
    }
    
    // countdown price model
    private class CountdownPriceResponseModel
    {   
        // price models for prices
        public class CountdownPriceModel
        {
            [JsonPropertyName("originalPrice")] public decimal? OriginalPrice { get; set; }
            [JsonPropertyName("salePrice")] public decimal? SalePrice { get; set; }
        }
        
        // size model for quantity
        public class CountdownSizeModel
        {
            [JsonPropertyName("volumeSize")] public string? VolumeSize { get; set; }
        }
        
        [JsonPropertyName("price")] public CountdownPriceModel? Price { get; set; }
        [JsonPropertyName("size")] public CountdownSizeModel? Size { get; set; }
    }
    
    public async Task<PriceModel> RetrievePrice(string storeProductCode)
    {

        var url = $"https://shop.countdown.co.nz/api/v1/products/{storeProductCode}";

        // add headers
        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        _client.DefaultRequestHeaders.Add("User-Agent",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:99.0) Gecko/20100101 Firefox/99.0");
        // client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
        _client.DefaultRequestHeaders.Add("ContentType", "application/json");
        _client.DefaultRequestHeaders.Add("X-Requested-With", "OnlineShopping.WebApp");

        var response = await _client.GetStringAsync(url);
        
        // deserialize into CountdownPriceResponseModel
        var countdownPrice = JsonSerializer.Deserialize<CountdownPriceResponseModel>(response);

        // required price information
        var originalPrice = countdownPrice?.Price?.OriginalPrice;
        var salePrice = countdownPrice?.Price?.SalePrice;
        var isOnSale = salePrice < originalPrice;
        var isAvailable = originalPrice > 0;
        var priceDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo
            .FindSystemTimeZoneById("New Zealand Standard Time")).Date;
        var priceQuantity = countdownPrice?.Size?.VolumeSize;
        
        // create price model
        var price = new PriceModel
        {
            OriginalPrice = originalPrice ?? 0,
            SalePrice = salePrice ?? 0,
            IsOnSale = isOnSale,
            IsAvailable = isAvailable,
            PriceDate = priceDate,
            PriceQuantity = priceQuantity ?? "none"
        };
        
        return price;
    }
}