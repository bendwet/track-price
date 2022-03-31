using System;
using System.Data.Common;
using System.Linq;
using Microsoft.Data.Sqlite;
using Xunit;
using SpendyDb.Models;
using SpendyDb.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace SpendyBackend.Tests;

public class InMemorySpendyDbControllerTest
{
    public readonly ITestOutputHelper Output;
    // private readonly DbContextOptions<SpendyContext> _contextOptions;
    public readonly SpendyContext Context;
    
    
    public InMemorySpendyDbControllerTest(ITestOutputHelper output)
    {   
        var services = new ServiceCollection()
            .AddDbContext<SpendyContext>((context, options) =>
            {
                var sqliteConnection = context.GetRequiredService<DbConnection>(); 
                options.UseSqlite(sqliteConnection);
            })
            .AddScoped(x =>
            {
                DbConnection sqliteConnection = new SqliteConnection("Filename=:memory:");
                sqliteConnection.Open();
                return sqliteConnection;
            })
            .BuildServiceProvider();
        
        Output = output;
        
        Context = services.GetRequiredService<SpendyContext>();
        
        Context.Database.EnsureDeleted();
        Context.Database.EnsureCreated();

        var store = new Store
        {
            StoreName = "countdown"
        };

        var product = new Product
        {
            ProductName = "Milk",
            UnitOfMeasure = "L",
            UnitOfMeasureSize = 2.0
        };

        var storeProduct = new StoreProduct
        {
            Store = store,
            Product = product,
            StoreProductCode = "123456"
        };

        var price = new Price
        {
            Store = store,
            Product = product,
            PriceDate = new DateTime(2022, 3, 29),
            PriceOriginal = 3.9,
            IsOnSale = false,
            PriceSale = 3.9,
            IsAvailable = true
        };
        
        Context.AddRange(
            store,
            product,
            storeProduct,
            price
        );
        
        Context.SaveChanges();

    }
    
    [Fact]
    // Query Store
    public void GetStore()
    {
        var store = Context.Stores.First().StoreName;
        
        Output.WriteLine(store);
        
    }
    
}