using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Spendy.Shared.Data;
using Spendy.Shared.Repositories;

namespace Spendy.Shared;

public static class DependencyInjectionConfiguration
{
    public static void AddSpendyServices(this IServiceCollection services, string connectionString)
    {
        services
            .AddScoped<IPriceRepository, PriceRepository>()
            .AddScoped<IStoreRepository, StoreRepository>()
            .AddScoped<IStoreProductRepository, StoreProductRepository>()
            .AddDbContext<SpendyContext>(options =>
            {
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });
    }
}