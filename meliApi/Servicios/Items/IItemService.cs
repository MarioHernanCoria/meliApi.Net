using meliApi.Entidades;

public interface IItemService
{
    Task<List<string>> GetItemsByUserIdAsync(int userId);
    Task<Producto> GetItemSpecificationsAsync(string itemId);
    Task<string> GetItemDescriptionAsync(string itemId);
    Task<string> GetMultipleItemSpecificationsAsync(string itemIds);
    Task<string> GetItemSpecificationsWithAttributesAsync(string itemIds, string attributes);
}

