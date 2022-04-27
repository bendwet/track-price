using System.ComponentModel.DataAnnotations.Schema;
#pragma warning disable CS8618

namespace SpendyDb.Models;

[Table("products")]
public record Product
{   
    [Column("product_id")]
    public int ProductId { get; init; }
    [Column("product_name")]
    public string ProductName { get; init; }
    [Column("unit_of_measure")]
    public string UnitOfMeasure { get; init; }
    [Column("unit_of_measure_size")]
    public double UnitOfMeasureSize { get; init; }
    
    public ICollection<StoreProduct> StoreProducts { get; init; }
    public ICollection<Price> Prices { get; init; }
}