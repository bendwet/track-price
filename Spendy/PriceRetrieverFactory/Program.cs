using PriceRetrieverFactory.PriceRetrievers;
using Microsoft.EntityFrameworkCore;

namespace PriceRetrieverFactory;

public class Program
{
    private static void Main()
    {
        var c = new CountdownPriceRetriever();
        var t = c.RequestPrice("282848");
        
        c.CreatePrice(t.Result);
    }
}


