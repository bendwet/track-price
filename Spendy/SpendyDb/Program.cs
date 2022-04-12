using Microsoft.EntityFrameworkCore;
using SpendyDb.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SpendyDb.Data;

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
        var services = new ServiceCollection()
            .AddDbContext<SpendyContext>(options =>
            {
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            })
            .AddLogging(x => x.AddConsole())
            .BuildServiceProvider();

    }
}
