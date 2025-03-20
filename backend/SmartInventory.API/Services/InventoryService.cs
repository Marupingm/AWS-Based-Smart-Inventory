using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using SmartInventory.API.Models;
using System.Text.Json;

namespace SmartInventory.API.Services;

public class InventoryService : IInventoryService
{
    private readonly IAmazonDynamoDB _dynamoDb;
    private readonly string _tableName;
    private readonly ILogger<InventoryService> _logger;

    public InventoryService(
        IAmazonDynamoDB dynamoDb,
        IConfiguration configuration,
        ILogger<InventoryService> logger)
    {
        _dynamoDb = dynamoDb;
        _tableName = configuration["AWS:DynamoDb:TableName"] ?? "SmartInventory";
        _logger = logger;
    }

    public async Task<IEnumerable<InventoryItem>> GetAllItemsAsync()
    {
        try
        {
            var request = new ScanRequest
            {
                TableName = _tableName
            };

            var response = await _dynamoDb.ScanAsync(request);
            return response.Items.Select(item => DeserializeFromDocument(item));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all inventory items");
            throw;
        }
    }

    public async Task<InventoryItem?> GetItemByIdAsync(string id)
    {
        try
        {
            var request = new GetItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    { "id", new AttributeValue { S = id } }
                }
            };

            var response = await _dynamoDb.GetItemAsync(request);
            return response.Item != null ? DeserializeFromDocument(response.Item) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving inventory item {Id}", id);
            throw;
        }
    }

    public async Task<InventoryItem> CreateItemAsync(InventoryItem item)
    {
        try
        {
            item.Id = Guid.NewGuid().ToString();
            item.CreatedAt = DateTime.UtcNow;
            item.UpdatedAt = DateTime.UtcNow;

            var request = new PutItemRequest
            {
                TableName = _tableName,
                Item = SerializeToDocument(item)
            };

            await _dynamoDb.PutItemAsync(request);
            return item;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating inventory item");
            throw;
        }
    }

    public async Task UpdateItemAsync(InventoryItem item)
    {
        try
        {
            item.UpdatedAt = DateTime.UtcNow;

            var request = new PutItemRequest
            {
                TableName = _tableName,
                Item = SerializeToDocument(item)
            };

            await _dynamoDb.PutItemAsync(request);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating inventory item {Id}", item.Id);
            throw;
        }
    }

    public async Task DeleteItemAsync(string id)
    {
        try
        {
            var request = new DeleteItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    { "id", new AttributeValue { S = id } }
                }
            };

            await _dynamoDb.DeleteItemAsync(request);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting inventory item {Id}", id);
            throw;
        }
    }

    private Dictionary<string, AttributeValue> SerializeToDocument(InventoryItem item)
    {
        return new Dictionary<string, AttributeValue>
        {
            { "id", new AttributeValue { S = item.Id } },
            { "name", new AttributeValue { S = item.Name } },
            { "quantity", new AttributeValue { N = item.Quantity.ToString() } },
            { "price", new AttributeValue { N = item.Price.ToString() } },
            { "category", new AttributeValue { S = item.Category } },
            { "supplier", new AttributeValue { S = item.Supplier } },
            { "createdAt", new AttributeValue { S = item.CreatedAt.ToString("O") } },
            { "updatedAt", new AttributeValue { S = item.UpdatedAt.ToString("O") } }
        };
    }

    private InventoryItem DeserializeFromDocument(Dictionary<string, AttributeValue> document)
    {
        return new InventoryItem
        {
            Id = document["id"].S,
            Name = document["name"].S,
            Quantity = int.Parse(document["quantity"].N),
            Price = decimal.Parse(document["price"].N),
            Category = document["category"].S,
            Supplier = document["supplier"].S,
            CreatedAt = DateTime.Parse(document["createdAt"].S),
            UpdatedAt = DateTime.Parse(document["updatedAt"].S)
        };
    }
} 