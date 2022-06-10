using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
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

            services.AddHttpClient<PaknsavePriceRetriever>()
                .ConfigureHttpClient(client =>
                {
                    client.DefaultRequestVersion = new Version(2, 0);
                    client.DefaultRequestHeaders.Add("user-agent", 
                        "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:101.0) Gecko/20100101 Firefox/101.0");
                    client.DefaultRequestHeaders.Add("accept-language", "en-US,en;q=0.9");
                    client.DefaultRequestHeaders.Add("accept-encoding", "*");
                    client.DefaultRequestHeaders.Add("cookie", 
                        "brands_store_id={815DCF68-9839-48AC-BF94-5F932A1254B5}; eCom_STORE_ID=65defcf2-bc15-490e-a84f-1f13b769cd22");
                });
            services.AddHttpClient<NewWorldPriceRetriever>()
                .ConfigureHttpClient(client =>
                {
                    client.DefaultRequestVersion = new Version(2, 0);
                    // client.DefaultRequestHeaders.Add("accept", 
                    //     "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,*/*;q=0.8");
                    // client.DefaultRequestHeaders.Add("accept-language", "en-US,en;q=0.5");
                    // client.DefaultRequestHeaders.Add("accept-encoding", "*");
                    client.DefaultRequestHeaders.Add("user-agent",
                        "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:101.0) Gecko/20100101 Firefox/101.0");
                });
            services.AddHttpClient<CountdownPriceRetriever>()
                .ConfigureHttpClient(client =>
                {
                    client.DefaultRequestVersion = new Version(2, 0);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("User-Agent",
                        "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:99.0) Gecko/20100101 Firefox/99.0");
                    // client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
                    client.DefaultRequestHeaders.Add("ContentType", "application/json");
                    client.DefaultRequestHeaders.Add("X-Requested-With", "OnlineShopping.WebApp");
                });
            
            var serviceProvider = services.BuildServiceProvider();

            })
            .AddScoped<BrowserFetcher>();

        // new random instance for random retry amount
        var r = new Random();

        serviceCollection.AddHttpClient<PaknsavePriceRetriever>()
            .ConfigureHttpClient(client =>
            {   
                // configure use of http2
                client.DefaultRequestVersion = new Version(2, 0);
            })
            .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(5, retryAttempt =>
                TimeSpan.FromSeconds(r.Next(1, 5) + retryAttempt)));
        
        var serviceProvider = serviceCollection.BuildServiceProvider();

        // Repositories
        var priceRepository = serviceProvider.GetRequiredService<IPriceRepository>();
        var storeRepository = serviceProvider.GetRequiredService<IStoreRepository>();
        var storeProductRepository = serviceProvider.GetRequiredService<IStoreProductRepository>();
        
        // Price retrievers
        var resolvePriceRetriever = serviceProvider.GetRequiredService<PriceRetrieverResolver>();

        // Get all store products to load
        var taskCompletionSource = new TaskCompletionSource();
        
        // store to be used for price retriever
        // var storeName = Environment.GetEnvironmentVariable("STORE");

        var storeName = "paknsave";
        
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
            var r = new Random();

            var policy = Policy
                .Handle<HttpRequestException>()
                .Or<TimeoutException>()
                .WaitAndRetryAsync(5,
                    _ => TimeSpan.FromMilliseconds(r.Next(30000, 35000)),
                    (exception, timespan) =>
                    {
                        Console.WriteLine($"Failed to retrieve price with error: {exception.Message}, " +
                                          $"retrying in {timespan}");
                    });
                
            // Retrieve price of store product, will not be null
            var priceRetriever = resolvePriceRetriever(storeName!);

            var price = await policy.ExecuteAsync(async () =>
            {
                PriceModel priceModel;
                try
                {
                    priceModel = await priceRetriever.RetrievePrice(sp.StoreProductCode);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error retrieving price : {ex.Message}, {ex}");
                    throw;
                }

                return priceModel;
            });

            // if (price == null)
            // {
            //     // TODO: Handle null
            // }

            var retryPolicy = Policy
                .Handle<HttpRequestException>()
                .WaitAndRetryAsync(
                    5, 
                    retryAttempt => TimeSpan.FromSeconds(r.Next(15, 20) * retryAttempt),
                    (exception, timespan) =>
                    {
                        Console.WriteLine($"Could not retrieve price with error: {exception.Message}, " +
                                          $"waiting {timespan} " + "before next attempt");
                    });
            
            var price = await retryPolicy.ExecuteAsync(() => priceRetriever.RetrievePrice(sp.StoreProductCode));
          
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


