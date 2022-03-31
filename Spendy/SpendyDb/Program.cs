using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using SpendyDb.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using MySql.EntityFrameworkCore.Extensions;
using MySqlConnector;
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
        var services = new ServiceCollection();
        services.AddDbContext<SpendyContext>(options =>
        {
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        });
        
    }
}
