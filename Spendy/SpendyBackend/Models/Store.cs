using System.ComponentModel.DataAnnotations;

namespace SpendyBackend.Models;

public class Store
{
    public int StoreId { get; set; }
    
    public string StoreName { get; set; }
    public ICollection<StoreProduct> StoreProducts { get; set; }
    public ICollection<Price> Prices { get; set; }
}
