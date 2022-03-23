using System.ComponentModel.DataAnnotations;

namespace SpendyDb.Models;

public class Store
{
    public int StoreId { get; set; }
    
    # nullable disable
    public string StoreName { get; set; }
    public ICollection<StoreProduct> StoreProducts { get; set; }
    public ICollection<Price> Prices { get; set; }
}
