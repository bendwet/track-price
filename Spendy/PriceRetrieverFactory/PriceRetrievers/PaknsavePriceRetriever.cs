using PriceRetrieverFactory.Interfaces;

namespace PriceRetrieverFactory.PriceRetrievers;

public class PaknsavePriceRetriever: IPriceRetriever
{
    private readonly HttpClient _client;

    public PaknsavePriceRetriever(HttpClient client)
    {
        _client = client;
    }
    
}