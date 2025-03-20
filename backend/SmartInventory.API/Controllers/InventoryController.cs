using Microsoft.AspNetCore.Mvc;
using SmartInventory.API.Models;
using SmartInventory.API.Services;

namespace SmartInventory.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _inventoryService;
    private readonly ILogger<InventoryController> _logger;

    public InventoryController(IInventoryService inventoryService, ILogger<InventoryController> logger)
    {
        _inventoryService = inventoryService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InventoryItem>>> GetAllItems()
    {
        try
        {
            var items = await _inventoryService.GetAllItemsAsync();
            return Ok(items);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all inventory items");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<InventoryItem>> GetItem(string id)
    {
        try
        {
            var item = await _inventoryService.GetItemByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting inventory item {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<ActionResult<InventoryItem>> CreateItem(InventoryItem item)
    {
        try
        {
            var createdItem = await _inventoryService.CreateItemAsync(item);
            return CreatedAtAction(nameof(GetItem), new { id = createdItem.Id }, createdItem);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating inventory item");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateItem(string id, InventoryItem item)
    {
        if (id != item.Id)
        {
            return BadRequest();
        }

        try
        {
            await _inventoryService.UpdateItemAsync(item);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating inventory item {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItem(string id)
    {
        try
        {
            await _inventoryService.DeleteItemAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting inventory item {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }
} 