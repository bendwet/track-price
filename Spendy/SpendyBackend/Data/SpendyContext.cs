using Microsoft.EntityFrameworkCore;
using SpendyBackend.Models;

namespace SpendyBackend.Data;

public class SpendyContext : DbContext
{
    public DbSet<Store> Stores { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<StoreProduct> StoreProducts { get; set; }
    public DbSet<Price> Prices { get; set; }

    public SpendyContext(DbContextOptions<SpendyContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Store>()
            .HasKey(x => x.StoreId);
        
        modelBuilder.Entity<Store>()
            .HasMany<StoreProduct>()
            .WithOne(x => x.Store)
            .HasForeignKey(x => x.StoreId);

        modelBuilder.Entity<Store>()
            .HasMany<Price>()
            .WithOne(x => x.Store)
            .HasForeignKey(x => x.StoreId);
        
        modelBuilder.Entity<Product>()
            .HasKey(x => x.ProductId);
        
        modelBuilder.Entity<Product>()
            .HasMany<StoreProduct>(x => x.StoreProducts)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId);
        
        modelBuilder.Entity<Product>()
            .HasMany<Price>(x => x.Prices)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId);

        modelBuilder.Entity<Price>()
            .HasKey(x => x.PriceId);
        
        // modelBuilder.Entity<Price>()
        //     .HasOne<Store>()
        //     .WithMany(x => x.Prices)
        //     .HasForeignKey(x => x.StoreId);
        
    }
}