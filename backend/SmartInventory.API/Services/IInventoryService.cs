using SmartInventory.API.Models;

namespace SmartInventory.API.Services;

public interface IInventoryService
{
    Task<IEnumerable<InventoryItem>> GetAllItemsAsync();
    Task<InventoryItem?> GetItemByIdAsync(string id);
    Task<InventoryItem> CreateItemAsync(InventoryItem item);
    Task UpdateItemAsync(InventoryItem item);
    Task DeleteItemAsync(string id);
} 