using System.Text.Json;
using System.Text.Json.Serialization;
using HtmlAgilityPack;
using PuppeteerSharp;
using System.Web;
using PriceRetriever.Interfaces;


namespace PriceRetriever.PriceRetrievers;

public class NewWorldPriceRetriever: IPriceRetriever
{
    private readonly BrowserFetcher _browserFetcher;
    public NewWorldPriceRetriever(BrowserFetcher browserFetcher)
    {
        _browserFetcher = browserFetcher;
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
        // var testUrl = $"https://www.newworld.co.nz/shop/product/{storeProductCode}_ea_000nw";
        
        const string setStoreLocation = "https://www.newworld.co.nz/CommonApi/Store/ChangeStore?storeId=773ad0a0-024e-46c5-a94b-df1cf86d25cc&clickSource=list";
        var url = $"https://www.newworld.co.nz/shop/Search?q={storeProductCode}";
        
        await _browserFetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
        
        var browser = await Puppeteer.LaunchAsync(new LaunchOptions
        {
            Headless = false,
            Args = new[] {"--no-sandbox"}
        });
        
        var searchPage = await browser.NewPageAsync();
        await searchPage.SetExtraHttpHeadersAsync(new Dictionary<string, string>
        {
            {"User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:100.0) Gecko/20100101 Firefox/100.0"}
        });
        
        // change store location
        await searchPage.GoToAsync(setStoreLocation);
        // search page via store product code
        var response = await searchPage.GoToAsync(url);
        // convert response to string
        var stringResponse = response.TextAsync().Result;

        // price info
        var isAvailable = true;
        var originalPrice = 0d;
        var salePrice = 0d;
        var isOnSale = false;
        var priceDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo
            .FindSystemTimeZoneById("New Zealand Standard Time")).Date;
        var priceQuantity = "none";
        
        
        // var response = await _client.GetStringAsync(url);
        var page = new HtmlDocument();

        page.LoadHtml(stringResponse);

        // get product availability
        var availabilityInfo = page.DocumentNode
            .Descendants()
            .FirstOrDefault(n => n.GetAttributeValue("class", "Product Found")
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
            
            salePrice = Convert.ToDouble(newWorldPrice?.ProductDetails?.CurrentPrice);
            
            // check if product is on sale
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