﻿using Microsoft.EntityFrameworkCore;
using Spendy.Shared.Models;

namespace Spendy.Shared.Data;

public class SpendyContext : DbContext
{
    public DbSet<Store> Stores => Set<Store>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<StoreProduct> StoreProducts => Set<StoreProduct>();
    public DbSet<Price> Prices => Set<Price>();
    public DbSet<Item> Items => Set<Item>();
    public DbSet<ProductIdItem> ProductIdItems => Set<ProductIdItem>();
    public DbSet<LowestPriceHistoryItem> LowestPriceDateItems => Set<LowestPriceHistoryItem>();

    public SpendyContext(DbContextOptions<SpendyContext> options) : base(options)
    {
        
    }
    
    // Create database model
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Store>()
            .HasKey(x => x.StoreId);

        modelBuilder.Entity<Store>()
            .HasMany(x => x.StoreProducts)
            .WithOne(x => x.Store)
            .HasForeignKey(x => x.StoreId);

        modelBuilder.Entity<StoreProduct>()
            .HasKey(x => x.StoreProductId);

         modelBuilder.Entity<Store>()
             .HasMany(x => x.Prices)
             .WithOne(x => x.Store)
             .HasForeignKey(x => x.StoreId);

        modelBuilder.Entity<Product>()
            .HasKey(x => x.ProductId);

        modelBuilder.Entity<Product>()
            .HasMany(x => x.StoreProducts)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId);

        modelBuilder.Entity<Product>()
            .HasMany(x => x.Prices)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId);

        modelBuilder.Entity<Price>()
            .HasKey(x => x.PriceId);
        
        // entity type for DTO query
        modelBuilder.Entity<Item>().HasNoKey();
        
        // entity type for item by product id DTO
        modelBuilder.Entity<ProductIdItem>().HasNoKey();
        
        // entity type for lowest price per date per product id DTO
        modelBuilder.Entity<LowestPriceHistoryItem>().HasNoKey();

        // modelBuilder.Entity<Price>()
        //     .HasOne(x => x.Product)
        //     .WithMany(x => x.Prices)
        //     .HasForeignKey(x => x.ProductId);
    }
}