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
            .AddDbContext<SpendyContext>(options =>
            {
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });
    }
}