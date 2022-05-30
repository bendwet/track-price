
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Spendy.Shared;
using Spendy.Shared.Repositories;

namespace SpendyDb;

public class Program
{
    private static void Main()
    {
        IConfiguration config = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();
        
        // Environment variable name for the connection string
        // ConnectionStrings__SpendyConnection

        var connectionString = config.GetConnectionString("SpendyConnection");
        Console.WriteLine(connectionString);
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSpendyServices(connectionString);
        
        serviceCollection.AddLogging(x => x.AddConsole());
            
        var services = serviceCollection.BuildServiceProvider();
        var context = services.GetRequiredService<IStoreRepository>();
        context.GetAllStores();
    }
}
