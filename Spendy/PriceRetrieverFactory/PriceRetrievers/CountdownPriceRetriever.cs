using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using SpendyDb.Models;

namespace PriceRetrieverFactory.PriceRetrievers;

public class CountdownPriceRetriever
{
    public class CountdownPriceResponseModel
    {
        public class CountdownPriceModel
        {
            [JsonPropertyName("originalPrice")]
            public decimal? OriginalPrice { get; set; }
            
            [JsonPropertyName("salePrice")]
            public decimal? SalePrice { get; set; }
        }
        
        [JsonPropertyName("price")]
        public CountdownPriceModel Price { get; set; }
    }
    
    // request the price of product with supplied product code
    public async Task<CountdownPriceResponseModel?> RequestPrice(string storeProductCode)
    {

        var client = new HttpClient();
        
        var url = $"https://shop.countdown.co.nz/api/v1/products/{storeProductCode}";

        // add headers
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:99.0) Gecko/20100101 Firefox/99.0");
        // client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
        client.DefaultRequestHeaders.Add("ContentType", "application/json");
        client.DefaultRequestHeaders.Add("X-Requested-With","OnlineShopping.WebApp");

        var response = await client.GetStringAsync(url);
        // deserialize into CountdownPriceResponseModel
        var price = JsonSerializer.Deserialize<CountdownPriceResponseModel>(response);
        return price;
    }

    public decimal? CreatePrice(CountdownPriceResponseModel countdownPrice)
    {   

        Console.WriteLine(countdownPrice.Price.OriginalPrice);

        // var price = new Price();
        // return price;
        return countdownPrice.Price.OriginalPrice;
    }
    
}