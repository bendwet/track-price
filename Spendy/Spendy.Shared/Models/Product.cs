using System.ComponentModel.DataAnnotations.Schema;

#pragma warning disable CS8618

namespace Spendy.Shared.Models;

[Table("products")]
public class Product
{   
    [Column("product_id")]
    public int ProductId { get; set; }
    [Column("product_name")]
    public string ProductName { get; set; }
    [Column("unit_of_measure")]
    public string UnitOfMeasure { get; set; }
    [Column("unit_of_measure_size")]
    public double UnitOfMeasureSize { get; set; }
    
    public ICollection<StoreProduct> StoreProducts { get; set; }
    public ICollection<Price> Prices { get; set; }
}