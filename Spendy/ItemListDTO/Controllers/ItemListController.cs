using Microsoft.AspNetCore.Mvc;
using ItemListDTO.Services;
using Spendy.Shared.Models;

namespace ItemListDTO.Controllers;

[ApiController]
[Route("[controller]")]
public class ItemListController: ControllerBase
{
    private readonly IItemListService _itemListService;

    public ItemListController(IItemListService itemListService)
    {
        _itemListService = itemListService;
    }

    [HttpGet]
    public ActionResult<List<Item>> GetAll() => _itemListService.GetItems();

}