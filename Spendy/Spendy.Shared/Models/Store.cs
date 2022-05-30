using System.ComponentModel.DataAnnotations.Schema;

#pragma warning disable CS8618

namespace Spendy.Shared.Models;

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
