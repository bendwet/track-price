﻿using PriceRetriever.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;
using HtmlAgilityPack;
using PuppeteerSharp;
using System.Web;
using PriceRetriever.Interfaces;
namespace PriceRetriever.PriceRetrievers;

public class PaknsavePriceRetriever: IWebScrapePriceRetriever
{
    private readonly HttpClient _client;

    public PaknsavePriceRetriever(HttpClient client)
    {
        _client = client;
    }
    
    private class PaknsavePriceResponseModel
    {
        public class PaknsaveSaleModel
        {
            [JsonPropertyName("PromoBadgeImageLabel")] public string? PromoLabel { get; set; }
        }
        [JsonPropertyName("PricePerItem")] public string? CurrentPrice { get; set; }
        [JsonPropertyName("ProductDetails")]public PaknsaveSaleModel? ProductDetails { get; set; }
    }
    
    public async Task<PriceModel> RetrievePrice(BrowserFetcher browserFetcher,string storeProductCode)
    {
        // var testUrl = $"https://www.newworld.co.nz/shop/product/{storeProductCode}_ea_000nw";
        
        const string setStoreLocation = "https://www.paknsave.co.nz/CommonApi/Store/ChangeStore?storeId=65defcf2-bc15-490e-a84f-1f13b769cd22&clickSource=list";
        var url = $"https://www.paknsave.co.nz/shop/Search?q={storeProductCode}";
        
        await browserFetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
        
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
        
        // go to search page
        await searchPage.GoToAsync(url);
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
            
            var decodedInfo = HttpUtility.HtmlDecode(priceInfo);
            var paknsavePrice = JsonSerializer.Deserialize<PaknsavePriceResponseModel>(decodedInfo);
            
            salePrice = Convert.ToDouble(paknsavePrice?.CurrentPrice);
            
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
            var quantity = page.DocumentNode
                .Descendants()
                .First(n => n.GetAttributeValue("class", "")
                    .Contains("u-color-half-dark-grey u-p3")).InnerText;
            
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