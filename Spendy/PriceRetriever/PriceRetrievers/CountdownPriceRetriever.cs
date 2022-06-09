using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using PriceRetriever.Interfaces;

namespace PriceRetriever.PriceRetrievers;

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
            [JsonPropertyName("originalPrice")] public double? OriginalPrice { get; set; }
            [JsonPropertyName("salePrice")] public double? SalePrice { get; set; }
        }
        
        // size model for quantity
        public class CountdownSizeModel
        {
            [JsonPropertyName("volumeSize")] public string? VolumeSize { get; set; }
            [JsonPropertyName("packageType")] public string? PackageType { get; set; }
        }
        
        [JsonPropertyName("name")] public string? Name { get; set; }
        [JsonPropertyName("price")] public CountdownPriceModel? Price { get; set; }
        [JsonPropertyName("size")] public CountdownSizeModel? Size { get; set; }
    }
    
    public async Task<PriceModel> RetrievePrice(string storeProductCode)
    {

        var url = $"https://shop.countdown.co.nz/api/v1/products/{storeProductCode}";
        
        // _client.DefaultRequestVersion = new Version(2, 0);
        // add headers
        // _client.DefaultRequestHeaders.Accept.Clear();
        // _client.DefaultRequestHeaders.Accept.Add(
        //     new MediaTypeWithQualityHeaderValue("application/json"));
        // _client.DefaultRequestHeaders.Add("User-Agent",
            // "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:99.0) Gecko/20100101 Firefox/99.0");
        // client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
        // _client.DefaultRequestHeaders.Add("ContentType", "application/json");
        // _client.DefaultRequestHeaders.Add("X-Requested-With", "OnlineShopping.WebApp");

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
        var volumeSize = countdownPrice?.Size?.VolumeSize;
        var packageType = countdownPrice?.Size?.PackageType;
        var name = countdownPrice?.Name;
        
        // get quantity of the product and check for edge case quantities
        // check volume size first
        var priceQuantity = volumeSize switch
        {
            "per kg" => "1kg",
            "1kg pack" => "1kg",
            "4 serve" => "4pk",
            // if volume size contains no valid quantities (is null) check package type
            _ => packageType switch
            {
                "each" when volumeSize == null => "ea",
                "bunch" when volumeSize == null => "bunch",
                _ => volumeSize
            }
        };

        // check name for toilet paper, as toilet paper will not have any quantity info in response
        if (name != null && name.Contains("toilet paper") && name.Contains("pk"))
        {
            try
            {
                priceQuantity = Regex.Match(name, "([0-9]+)(pk)").ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine($"The following error occured when attempting to get quantity from name: {e}");
                throw;
            }
        }
        
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