using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpendyBackend.Models;

[Table("stores")]
public class Store
{   
    [Column("store_id")]
    public int StoreId { get; set; }
    
    [Column("store_name")]
    public string StoreName { get; set; }
    public ICollection<StoreProduct> StoreProducts { get; set; }
    public ICollection<Price> Prices { get; set; }
}
