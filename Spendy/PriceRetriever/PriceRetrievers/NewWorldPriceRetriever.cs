using System.Text.Json;
using System.Text.Json.Serialization;
using HtmlAgilityPack;
using PuppeteerSharp;
using System.Web;
using System.Net;
using Spendy.Shared.Interfaces;


namespace PriceRetriever.PriceRetrievers;

public class NewWorldPriceRetriever: IPriceRetriever
{
    private readonly BrowserFetcher _browserFetcher;
    private readonly HttpClient _client;
    public NewWorldPriceRetriever(BrowserFetcher browserFetcher, HttpClient client)
    {
        _browserFetcher = browserFetcher;
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
        // await searchPage.GoToAsync(setStoreLocation);

        // search page via store product code
        var response = await searchPage.GoToAsync(url);

        var cookie = await searchPage.GetCookiesAsync();

        var cf = cookie.First(c => c.Name == "__cf_bm");
        
        Console.WriteLine(cf.Value);

        _client.DefaultRequestVersion = new Version(2, 0);
        _client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:101.0) Gecko/20100101 Firefox/101.0");
        _client.DefaultRequestHeaders.Add("cookie", @"Cookie: SC_ANALYTICS_GLOBAL_COOKIE=e8fc6adb07cc4d52a12b20d25f4bd4c5|False; STORE_ID_V2=773ad0a0-024e-46c5-a94b-df1cf86d25cc|False; brands_server_nearest_store={""StoreId"":""{A5188315-D432-45A0-8CAD-289EB96C70D9}"",""UserLat"":""-33.8715"",""UserLng"":""151.2006"",""StoreLat"":""-45.0254262291741"",""StoreLng"":""168.742751035582"",""IsSuccess"":true}; eCom_STORE_ID=773ad0a0-024e-46c5-a94b-df1cf86d25cc; brands_store_id={FBDC00D1-5B7A-46F6-AE85-68AD2BE4C1C1}; region_code=UNI; eComm_Coordinate_Cookie={""latitude"":-36.728169397933982,""longitude"":174.71053607832641}; eCom_StoreId_NotSameAs_Brands=false; Region=NI; brands_store_reset=; AllowRestrictedItems=true; server_nearest_store_v2={""StoreId"":""773ad0a0-024e-46c5-a94b-df1cf86d25cc"",""UserLat"":""-36.728169397934"",""UserLng"":""174.710536078326"",""StoreLat"":""-36.728207"",""StoreLng"":""174.710519"",""IsSuccess"":true}; SessionCookieIdV2=761bc4b5cdf04a128d076df8c27415e0; ASP.NET_SessionId=qwf4lqzlj12gy5rgldthcee3; shell#lang=en; __RequestVerificationToken=rwi7iIsgFf0AcxSCycWKm2x9mlVcV3GHhFQlHNxaxhEOTeibLWbA9pAjnWejCylD-Qi_6YJDVe7tlBvLqiOKvMxATXI1; sxa_site=New World; fs-new-feature-onboarding-closed=false; __cfruid=6bd87a690e1de0592305206d726e1043c82a5b6e-1654386380; __cf_bm=" + cf);
        var t = await _client.GetAsync(url);
        
        Console.WriteLine(t.RequestMessage.Version);
        
        Console.WriteLine(t.StatusCode);
        
        if (response.Status != HttpStatusCode.OK)
        {
            await browser.CloseAsync();
            throw new HttpRequestException("403 Forbidden");
        }
        
        // convert response to string
        var stringResponse = response.TextAsync().Result;
        await browser.CloseAsync();

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
            priceQuantity = page.DocumentNode
                .Descendants()
                .First(n => n.GetAttributeValue("class", "none")
                    .Contains("u-margin-right")).InnerText;
            
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