using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySqlX.XDevAPI;
using Polly;
using PriceRetriever.PriceRetrievers;
using SpendyDb.Models;
using SpendyDb.Repositories;

namespace PriceRetriever;

public class Program
{
    private static async Task Main()
    {
        IConfiguration config = new ConfigurationBuilder()
            .Build();

        var services = new ServiceCollection()
            .AddScoped<CountdownPriceRetriever>()
            .AddScoped<NewWorldPriceRetriever>()
            .AddScoped<IPriceRepository, PriceRepository>()
            .AddScoped<IStoreRepository, StoreRepository>()
            .AddScoped<IStoreProductRepository, StoreProductRepository>();

        // new random instance for random retry amount
        var r = new Random();

        services.AddHttpClient<CountdownPriceRetriever>()
            .ConfigureHttpClient(client =>
            {   
                // configure use of http2
                client.DefaultRequestVersion = new Version(2, 0);
            })
            .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(5, retryAttempt =>
                TimeSpan.FromSeconds(r.Next(1, 5) + retryAttempt)));

        var serviceProvider = services.BuildServiceProvider();

        // var countdownPriceRetriever = serviceProvider.GetRequiredService<CountdownPriceRetriever>();

        // retrieve price
        // var price = await countdownPriceRetriever.RetrievePrice("881157");

        // Repositories
        // var priceRepository = serviceProvider.GetRequiredService<IPriceRepository>();
        // var storeRepository = serviceProvider.GetRequiredService<IStoreRepository>();
        // var storeProductRepository = serviceProvider.GetRequiredService<IStoreProductRepository>();

        var nw = serviceProvider.GetRequiredService<NewWorldPriceRetriever>();

        await nw.RetrievePrice("5201479");

        // Get the stores
        // var stores = storeRepository.GetAllStores();
        //
        // foreach (var store in stores)
        // {   
        //     // retrieve all store products for certain store
        //     var storeProducts = storeProductRepository.RetrieveByStoreId(store.StoreId);
        //     
        //     // default price retriever is countdown
        //     var priceRetriever = serviceProvider.GetRequiredService<CountdownPriceRetriever>();
        //     // if store name = paknsave
        //         // var priceRetriever = paknsavePriceRetriever
        //     // elif store name = new world
        //         // var priceRetriever = newWorldPriceRetriever 
        //         
        //     foreach (var storeProduct in storeProducts)
        //     {   
        //         // retrieve price
        //         var price = await priceRetriever.RetrievePrice(storeProduct.StoreProductCode);
        //         
        //         // create price record for database
        //         var priceRecord = new Price
        //         {
        //             ProductId = storeProduct.ProductId,
        //             StoreId = storeProduct.StoreId,
        //             OriginalPrice = price.OriginalPrice,
        //             SalePrice = price.SalePrice,
        //             IsOnSale = price.IsOnSale,
        //             IsAvailable = price.IsAvailable,
        //             PriceDate = price.PriceDate,
        //             PriceQuantity = price.PriceQuantity
        //         };
        //         
        //         // save price record to database
        //         priceRepository.Save(priceRecord);
        //     }
        // }

        // Get store product for store id and product id
        // var storeProduct = storeProductRepository.RetrieveByStoreProductCode("282765", 1);
        //
        // // Create the price record to be saved into database
        // var priceRecord = new Price
        // {
        //     ProductId = storeProduct.ProductId,
        //     StoreId = storeProduct.StoreId,
        //     OriginalPrice = price.OriginalPrice,
        //     SalePrice = price.SalePrice,
        //     IsOnSale = price.IsOnSale,
        //     IsAvailable = price.IsAvailable,
        //     PriceDate = price.PriceDate,
        //     PriceQuantity = price.PriceQuantity
        // };
        //     
        // priceRepository.Save(priceRecord);
    }
}


