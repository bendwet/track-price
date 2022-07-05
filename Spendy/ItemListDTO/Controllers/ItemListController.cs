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
    
    [Route("")]
    [HttpGet]
    public ActionResult<List<Item>> GetItems() => new JsonResult(_itemListService.GetItems());
    
    [Route("{productId:int}")]
    [HttpGet]
    public ActionResult<List<Item>> GetItemByProductId(int productId) => new JsonResult(_itemListService.GetItemByProductId(productId));

}