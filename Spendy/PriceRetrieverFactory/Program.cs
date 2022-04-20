using PriceRetrieverFactory.PriceRetrievers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            .AddScoped<IPriceRepository, PriceRepository>();

        services.AddHttpClient<CountdownPriceRetriever>();

        var serviceProvider = services.BuildServiceProvider();

        var countdownPriceRetriever = serviceProvider.GetRequiredService<CountdownPriceRetriever>();
        
        // retrieve price
        var price = await countdownPriceRetriever.RetrievePrice("282848");
        
        // Save the price
        var priceRepository = serviceProvider.GetRequiredService<IPriceRepository>();
        
        // Find the store
        var priceRecord = new Price
        {

        };
            
        // priceRepository.Save(priceRecord);

        // c.CreatePrice(t.Result);

    }
}


