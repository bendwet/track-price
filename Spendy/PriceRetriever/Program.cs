using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySqlX.XDevAPI;
using Polly;
using PriceRetriever.Interfaces;
using PriceRetriever.PriceRetrievers;
using SpendyDb.Models;
using SpendyDb.Repositories;
using PuppeteerSharp;

namespace PriceRetriever;

public class Program
{   
    private delegate IPriceRetriever PriceRetrieverResolver(string key);
    private static async Task Main()
    {

        var services = new ServiceCollection()
            .AddScoped<CountdownPriceRetriever>()
            .AddScoped<NewWorldPriceRetriever>()
            .AddScoped<PaknsavePriceRetriever>()
            .AddScoped<PriceRetrieverResolver>(serviceProvider => key =>
            {
                return key switch
                {
                    "countdown" => serviceProvider.GetRequiredService<CountdownPriceRetriever>(),
                    "paknsave" => serviceProvider.GetRequiredService<PaknsavePriceRetriever>(),
                    "new world" => serviceProvider.GetRequiredService<NewWorldPriceRetriever>(),
                    _ => throw new KeyNotFoundException()
                };
            })
            .AddScoped<BrowserFetcher>()
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
        var priceRepository = serviceProvider.GetRequiredService<IPriceRepository>();
        var storeRepository = serviceProvider.GetRequiredService<IStoreRepository>();
        var storeProductRepository = serviceProvider.GetRequiredService<IStoreProductRepository>();
        
        // Price retrievers
        var resolvePriceRetriever = serviceProvider.GetRequiredService<PriceRetrieverResolver>();
        
        // var nw = serviceProvider.GetRequiredService<PaknsavePriceRetriever>();
        //
        // await nw.RetrievePrice("5201479");
        
        // Get all store products to load
        var taskCompletionSource = new TaskCompletionSource();
        
        // store to be used for price retriever
        var storeName = Environment.GetEnvironmentVariable("STORE");
        
        // Add all store products to queue, will not be null as will always be called with argument
        var store = storeRepository.GetByName(storeName!);

        var queue = new Queue<StoreProduct>();
        var storeProducts = storeProductRepository.GetByStoreId(store.StoreId);
        foreach (var sp in storeProducts)
        {
            queue.Enqueue(sp);
        }

        // While queue is not empty
        async Task SavePrice(StoreProduct sp)
        {
            // Retrieve price of store product
            var priceRetriever = resolvePriceRetriever(storeName!);


            var price = await priceRetriever.RetrievePrice(sp.StoreProductCode);
            var priceRecord = new Price
            {
                ProductId = sp.ProductId,
                StoreId = sp.StoreId,
                OriginalPrice = price.OriginalPrice,
                SalePrice = price.SalePrice,
                IsOnSale = price.IsOnSale,
                IsAvailable = price.IsAvailable,
                PriceDate = price.PriceDate,
                PriceQuantity = price.PriceQuantity
            };
            
            // save price to database
            priceRepository.Save(priceRecord);
            
        }
        
        async Task ScheduleNextRetrieval()
        {
            // If items in queue, then schedule
            if (queue.Count > 0)
            {
                var storeProduct = queue.Dequeue();
                await SavePrice(storeProduct);
                
                var retrieveDelay = new Random().Next(30000, 35000);
                // No need to await, as we want the task to run completely independently
#pragma warning disable CS4014
                Task.Delay(retrieveDelay)
                    .ContinueWith(task => ScheduleNextRetrieval);
#pragma warning restore CS4014

            }
            else
            {
                // If no longer items in queue, complete 
                taskCompletionSource.SetResult();
            }
        }

        var startDelay = new Random().Next(3000, 3500);
#pragma warning disable CS4014
        Task.Delay(startDelay)
            .ContinueWith(task => ScheduleNextRetrieval);
#pragma warning restore CS4014
        
        // Task.Delay(random delay).ContinueWith(RetrievePrice).ContinueWith(ScheduleNextRetrieval)
        taskCompletionSource.Task.GetAwaiter().GetResult();

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
        //          Thread.Sleep(randomMs);
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


