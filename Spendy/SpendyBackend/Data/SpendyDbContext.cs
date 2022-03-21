using Microsoft.EntityFrameworkCore;
using SpendyBackend.Models;

namespace SpendyBackend.Data;

public class SpendyDbContext : DbContext
{
    public DbSet<Store> Stores { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<StoreProduct> StoreProducts { get; set; }
    public DbSet<Price> Prices { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySQL("PlaceHolder");
    }
}