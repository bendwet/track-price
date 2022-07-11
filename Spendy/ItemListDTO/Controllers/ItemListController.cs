using Microsoft.AspNetCore.Mvc;
using ItemListDTO.Services;
using Spendy.Shared.Models;

namespace ItemListDTO.Controllers;

[ApiController]
[Route("items")]
public class ItemListController: ControllerBase
{
    private readonly IItemListService _itemListService;

    public ItemListController(IItemListService itemListService)
    {
        _itemListService = itemListService;
    }
    
    // get all items
    [Route("")]
    [HttpGet]
    public ActionResult<List<Item>> GetItems() => new JsonResult(_itemListService.GetItems());
    
    // get items by product id
    [Route("{productId:int}")]
    [HttpGet]
    public ActionResult<List<ProductIdItem>> GetItemByProductId(int productId) => 
        new JsonResult(_itemListService.GetItemByProductId(productId));
    
    // get lowest price item for each date for specific product id
    [Route("{productId:int}/price-history")]
    [HttpGet]
    public ActionResult<List<LowestPriceHistoryItem>> GetLowestPriceItemPerDate(int productId) =>
        new JsonResult(_itemListService.GetLowestPriceItemPerDate(productId));

}