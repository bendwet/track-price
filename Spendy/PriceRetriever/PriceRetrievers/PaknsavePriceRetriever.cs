using System.Drawing;
using System.Net;
using PriceRetriever.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;
using HtmlAgilityPack;
using PuppeteerSharp;
using System.Web;
using System.Net.Http.Headers;

namespace PriceRetriever.PriceRetrievers;

public class PaknsavePriceRetriever: IPriceRetriever
{
    private readonly BrowserFetcher _browserFetcher;
    private readonly HttpClient _client;
    
    public PaknsavePriceRetriever(BrowserFetcher browserFetcher, HttpClient client)
    {
        _browserFetcher = browserFetcher;
        _client = client;
    }
    
    private class PaknSavePriceResponseModel
    {
        public class PaknSavePriceModel
        {
            [JsonPropertyName("PromoBadgeImageLabel")] public string? PromoLabel { get; set; }
            [JsonPropertyName("PricePerItem")] public string? CurrentPrice { get; set; }
        }
        [JsonPropertyName("ProductDetails")]public PaknSavePriceModel? ProductDetails { get; set; }
    }
    
    public async Task<PriceModel> RetrievePrice(string storeProductCode)
    {
        // var testUrl = $"https://www.newworld.co.nz/shop/product/{storeProductCode}_ea_000nw";
        
        const string setStoreLocation = "https://www.paknsave.co.nz/CommonApi/Store/ChangeStore?storeId=65defcf2-bc15-490e-a84f-1f13b769cd22&clickSource=list";
        var url = $"https://www.paknsave.co.nz/shop/Search?q={storeProductCode}";
        
        // await _browserFetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
        //
        // var browser = await Puppeteer.LaunchAsync(new LaunchOptions
        // {
        //     Headless = false,
        //     Args = new[] {"--no-sandbox", "--start-maximized"}
        // });
        
        // var searchPage = await browser.NewPageAsync();
        // add headers
        // await searchPage.SetExtraHttpHeadersAsync(new Dictionary<string, string>
        // {
        //     {"User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:100.0) Gecko/20100101 Firefox/100.0"}
        // });
        // go to search page
        // var temp = await searchPage.GoToAsync(url);
        // var getCookie = await searchPage.GetCookiesAsync();
        //
        // var he = temp.Request.Headers;
        //
        // var cf = getCookie.First(c => c.Name == "__cf_bm").Value;
        //
        // Console.WriteLine(cf);
        //
        // Console.WriteLine(cookie);
        
        // _client.DefaultRequestHeaders.Clear();
        _client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:101.0) Gecko/20100101 Firefox/101.0");
        // _client.DefaultRequestHeaders.Add("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
        _client.DefaultRequestHeaders.Add("accept-language", "en-US,en;q=0.9");
        _client.DefaultRequestHeaders.Add("accept-encoding", "*");
        _client.DefaultRequestHeaders.Add("cookie", "brands_store_id={815DCF68-9839-48AC-BF94-5F932A1254B5}; eCom_STORE_ID=65defcf2-bc15-490e-a84f-1f13b769cd22");

        var response = await _client.GetAsync(url);

        // Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        
        // change store location
        // await searchPage.GoToAsync(url);
        // await searchPage.WaitForSelectorAsync("button[id='storeAction'");
        // await searchPage.ClickAsync("button[id='storeAction'");
        // await searchPage.FocusAsync("input[aria-label='Search address'");
        // await searchPage.Keyboard.TypeAsync("albany");

        // await searchPage.GoToAsync(url);
        // await searchPage.GoToAsync(setStoreLocation);
        
        // var response = await searchPage.GoToAsync(url);
        
        // Console.WriteLine(searchPage.Viewport.Height);
        // Console.WriteLine(searchPage.Viewport.Width);
        
        
        // var locationButton = await searchPage.QuerySelectorAsync("button[id='storeAction'");
        // var location = locationButton.BoundingBoxAsync();
        // var xValue = (int) location.Result.X;
        // var yValue = (int) location.Result.Y;
        // Console.WriteLine(xValue);
        // Console.WriteLine(yValue);
        
        // await searchPage.ClickAsync("button[id=storeAction");
        //
        // var r = new Random();
        // var point = new Point(200, 200);
        // var delay = TimeSpan.FromSeconds(r.Next(2, 4));
        // CursorMover.MoveCursor(point, delay);
        //
        // await searchPage.ClickAsync("button[id=storeAction");
        
        // open new tab
        // var newTab = await browser.NewPageAsync();
        // add headers to new tab
        // await newTab.SetExtraHttpHeadersAsync(new Dictionary<string, string>
        // {
        //     {"User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:100.0) Gecko/20100101 Firefox/100.0"}
        // });
        // search page via store product code
        // var response = await newTab.GoToAsync(url);

        // convert response to string
        var stringResponse = response.Content.ReadAsStringAsync().Result;
        // await browser.CloseAsync();
        
        if (response.StatusCode == HttpStatusCode.Forbidden)
        {
            // await browser.CloseAsync();
            throw new HttpRequestException("403 Forbidden");
        }
        
        // Console.WriteLine(stringResponse);

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
                .Contains("fs-search-result-header__title"))?.InnerText;
        
        // check is product is not available
        if (availabilityInfo == "Product not found")
        {
            isAvailable = false;
        }
        else
        {
            var priceInfo = page.DocumentNode
                .Descendants()
                .First(n => n.GetAttributeValue("class", "")
                    .Contains("js-product-card-footer fs-product-card__footer-container"))
                // .First(n => n.HasClass("js-product-card-footer fs-product-card__footer-container"))
                .Attributes["data-options"].Value;
            
            // Console.WriteLine(priceInfo);
            
            var decodedInfo = HttpUtility.HtmlDecode(priceInfo);
            var paknsavePrice = JsonSerializer.Deserialize<PaknSavePriceResponseModel>(decodedInfo);
            
            salePrice = Convert.ToDouble(paknsavePrice?.ProductDetails?.CurrentPrice);
            
            // check if product is on sale
            if (paknsavePrice?.ProductDetails?.PromoLabel is "Extra Low")
            {
                isOnSale = true;
            }
            else
            {
                originalPrice = salePrice;
            }
            
            // get quantity of product
            priceQuantity = page.DocumentNode
                .Descendants()
                .First(n => n.GetAttributeValue("class", "")
                    .Contains("u-color-half-dark-grey u-p3")).InnerText;
            
            // capitalize L symbol for consistency of measurement values
            if (priceQuantity.Contains('l'))
            {
                priceQuantity = priceQuantity.Replace("l", "L");
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