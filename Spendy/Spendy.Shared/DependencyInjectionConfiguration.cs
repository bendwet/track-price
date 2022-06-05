using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Spendy.Shared.Data;
using Spendy.Shared.Repositories;
using Spendy.Shared.Interfaces;
using PuppeteerSharp;

namespace Spendy.Shared;

public static class DependencyInjectionConfiguration
{
    public static void AddSpendyServices(this IServiceCollection services, string connectionString)
    {
        services
            .AddScoped<BrowserFetcher>()
            .AddScoped<IPriceRepository, PriceRepository>()
            .AddScoped<IStoreRepository, StoreRepository>()
            .AddScoped<IStoreProductRepository, StoreProductRepository>()
            .AddDbContext<SpendyContext>(options =>
            {
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });
    }

    public static void AddSpendyHttpClient(this IServiceCollection services)
    {
        services.AddHttpClient<IPriceRetriever>();
        // .ConfigureHttpClient(client =>
        // {
        //     // configure use of http2
        //     client.DefaultRequestVersion = new Version(2, 0); 
        // }); 
    }

}