using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#pragma warning disable CS8618

namespace SpendyDb.Models;

[Table("stores")]
public record Store
{   
    [Column("store_id")]
    public int StoreId { get; init; }
    
    [Column("store_name")]
    public string StoreName { get; init; }
    public ICollection<StoreProduct> StoreProducts { get; init; }
    public ICollection<Price> Prices { get; init; }
}
