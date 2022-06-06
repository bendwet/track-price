﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using PriceRetriever.PriceRetrievers;
using PriceRetriever.Interfaces;
using Spendy.Shared;
using Spendy.Shared.Models;
using Spendy.Shared.Repositories;

namespace PriceRetriever;

public class Program
{   
    private delegate IPriceRetriever PriceRetrieverResolver(string key);
    private static async Task Main()
    {
        IConfiguration config = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();
        
        // Environment variable name for the connection string
        // ConnectionStrings__SpendyConnection

        var connectionString = config.GetConnectionString("SpendyConnection");

        var services = new ServiceCollection();

        services.AddSpendyServices(connectionString);

        services
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
            });

            services.AddHttpClient<PaknsavePriceRetriever>();
            services.AddHttpClient<NewWorldPriceRetriever>();
            services.AddHttpClient<CountdownPriceRetriever>();
            
            var serviceProvider = services.BuildServiceProvider();

        // Repositories
        var priceRepository = serviceProvider.GetRequiredService<IPriceRepository>();
        var storeRepository = serviceProvider.GetRequiredService<IStoreRepository>();
        var storeProductRepository = serviceProvider.GetRequiredService<IStoreProductRepository>();
        
        // Price retrievers
        var resolvePriceRetriever = serviceProvider.GetRequiredService<PriceRetrieverResolver>();

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
            
            Console.WriteLine($"Product id: {sp.ProductId}");
            
            var r = new Random();

            var policy = Policy
                .Handle<HttpRequestException>()
                .WaitAndRetryAsync(5,
                    _ => TimeSpan.FromMilliseconds(r.Next(30000, 35000)),
                    (exception, timespan) =>
                    {
                        Console.WriteLine($"Failed to retrieve price with error: {exception}, retrying in {timespan}");
                    });
            
            // Retrieve price of store product, will not be null
            var priceRetriever = resolvePriceRetriever(storeName!);

            var price = await policy.ExecuteAsync(async () =>
            {
                PriceModel priceModel = null!;
                try
                {
                    priceModel = await priceRetriever.RetrievePrice(sp.StoreProductCode);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error retrieving price : {ex.Message}", ex);
                }

                return priceModel;
            });

            // if (price == null)
            // {
            //     // TODO: Handle null
            // }

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
                    .ContinueWith(task => ScheduleNextRetrieval());
#pragma warning restore CS4014

            }
            else
            {
                // If no longer items in queue, complete 
                taskCompletionSource.SetResult();
            }
        }

        var startDelay = new Random().Next(1000, 10000);
#pragma warning disable CS4014
        Task.Delay(startDelay)
            .ContinueWith(task => ScheduleNextRetrieval());
#pragma warning restore CS4014
        
        taskCompletionSource.Task.GetAwaiter().GetResult();
    }
}


