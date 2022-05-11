using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Web;
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
        public class NewWorldPriceModel
        {
            [JsonPropertyName("MultiBuyBasePrice")] public string? CurrentPrice { get; set; }
            [JsonPropertyName("PromoBadgeImageLabel")] public string? PromoLabel { get; set; }
        }
        [JsonPropertyName("ProductDetails")]public NewWorldPriceModel? ProductDetails { get; set; }
        
    }

    public async Task<PriceModel> RetrievePrice(string storeProductCode)
    {
        // var url = $"https://www.newworld.co.nz/shop/product/{storeProductCode}_ea_000nw";

        var url = $"https://www.newworld.co.nz/shop/Search?q={storeProductCode}";
        
        // add headers
        _client.DefaultRequestHeaders.Add("User-Agent",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:100.0) Gecko/20100101 Firefox/100.0");
        // _client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8");
        // client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
        // _client.DefaultRequestHeaders.Add("accept-language", "en-US,en;q=0.9");
        _client.DefaultRequestHeaders.Add("Cookie", NewWorldCookie.Cookie);
        
        // price info
        var isAvailable = true;
        var originalPrice = 0d;
        var salePrice = 0d;
        var isOnSale = false;
        var priceDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo
            .FindSystemTimeZoneById("New Zealand Standard Time")).Date;
        var priceQuantity = "none";
            
        var response = await _client.GetStringAsync(url);
        var page = new HtmlDocument();
        page.LoadHtml(response);
        
        // get product availability
        var availabilityInfo = page.DocumentNode
            .Descendants()
            .FirstOrDefault(n => n.GetAttributeValue("class", "")
                .Contains("fs-search-result-header__title u-margin-bottom"))?.InnerText;
        
        // check is product is not available
        if (availabilityInfo == "Product not found")
        {
            isAvailable = false;
        }
        else
        {
            var priceInfo = page.DocumentNode
                .Descendants()
                .First(n => n.HasClass("js-product-card-footer"))
                .Attributes["data-options"].Value;
            var decodedInfo = HttpUtility.HtmlDecode(priceInfo);
            var newWorldPrice = JsonSerializer.Deserialize<NewWorldPriceResponseModel>(decodedInfo);
            
            // check if product is on sale
            salePrice = Convert.ToDouble(newWorldPrice?.ProductDetails?.CurrentPrice);
            if (newWorldPrice?.ProductDetails?.PromoLabel is "Super Saver" or "Saver")
            {
                isOnSale = true;
            }
            else
            {
                originalPrice = salePrice;
            }
            
            // get quantity of product
            var quantity = page.DocumentNode
                .Descendants()
                .First(n => n.GetAttributeValue("class", "")
                    .Contains("u-margin-right")).InnerText;
            
            // capitalize L symbol for consistency of measurement values
            if (quantity.Contains('l'))
            {
                priceQuantity = quantity.Replace("l", "L");
            }
        }
        
        // create price model
        var price = new PriceModel
        {
            OriginalPrice = originalPrice,
            SalePrice = salePrice,
            IsOnSale = isOnSale,
            IsAvailable = isAvailable,
            PriceDate = priceDate,
            PriceQuantity = priceQuantity
        };
        
        return price;
    }
}