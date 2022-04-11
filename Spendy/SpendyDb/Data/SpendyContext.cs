﻿using Microsoft.EntityFrameworkCore;
using SpendyDb.Models;

namespace SpendyDb.Data;

public class SpendyContext : DbContext
{
    public DbSet<Store> Stores { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<StoreProduct> StoreProducts { get; set; }
    //public DbSet<Price> Prices { get; set; }

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
        
        // modelBuilder.Entity<Price>()
        //     .HasOne(x => x.Product)
        //     .WithMany(x => x.Prices)
        //     .HasForeignKey(x => x.ProductId);
    }
}