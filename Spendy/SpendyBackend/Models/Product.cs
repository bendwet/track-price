namespace SpendyBackend.Models;

public class Product
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string UnitOfMeasure { get; set; }
    public double UnitOfMeasureSize { get; set; }
    
    public ICollection<StoreProduct> StoreProducts { get; set; }
    public ICollection<Price> Prices { get; set; }
}