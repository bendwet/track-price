using PriceRetrieverFactory.PriceRetrievers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using SpendyDb.Models;
using SpendyDb.Repositories;

namespace PriceRetrieverFactory;

public class Program
{
    private static async Task Main()
    {
        IConfiguration config = new ConfigurationBuilder()
            .Build();
    
        var services = new ServiceCollection()
            .AddScoped<CountdownPriceRetriever>()
            .AddScoped<IPriceRepository, PriceRepository>()
            .AddScoped<IStoreRepository, StoreRepository>()
            .AddScoped<IStoreProductRepository, StoreProductRepository>();
        
        // new random instance for random retry amount
        var r = new Random();

        services.AddHttpClient<CountdownPriceRetriever>()
            .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(5, retryAttempt =>
                TimeSpan.FromSeconds(r.Next(1, 5) + retryAttempt)));
        
        var serviceProvider = services.BuildServiceProvider();

        var countdownPriceRetriever = serviceProvider.GetRequiredService<CountdownPriceRetriever>();
        
        // retrieve price
        var price = await countdownPriceRetriever.RetrievePrice("282765");
        
        // Repositories
        var priceRepository = serviceProvider.GetRequiredService<IPriceRepository>();
        var storeRepository = serviceProvider.GetRequiredService<IStoreRepository>();
        var storeProductRepository = serviceProvider.GetRequiredService<IStoreProductRepository>();

        // Get the stores
        var stores = storeRepository.GetAllStores();
        
        // Get store product for store id and product id
        var storeProduct = storeProductRepository.GetByStoreProductCode("282765", 1);
        
        // Create the price record to be saved into database
        var priceRecord = new Price
        {
            ProductId = storeProduct.ProductId,
            StoreId = storeProduct.StoreId,
            OriginalPrice = price.OriginalPrice,
            SalePrice = price.SalePrice,
            IsOnSale = price.IsOnSale,
            IsAvailable = price.IsAvailable,
            PriceDate = price.PriceDate,
            PriceQuantity = price.PriceQuantity
        };
            
        // priceRepository.Save(priceRecord);

        // c.CreatePrice(t.Result);

    }
}


