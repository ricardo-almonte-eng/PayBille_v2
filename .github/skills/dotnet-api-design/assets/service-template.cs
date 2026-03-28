using PayBille.Core.Models;
using PayBille.Data.Repositories;

namespace PayBille.Services
{
    public interface IItemService
    {
        Task<ItemResponse> CreateItemAsync(CreateItemRequest request);
        Task<ItemResponse> GetItemAsync(string id);
        Task<IEnumerable<ItemResponse>> ListItemsAsync();
    }

    public class ItemService : IItemService
    {
        private readonly IItemRepository _repository;
        private readonly ILogger<ItemService> _logger;

        public ItemService(IItemRepository repository, ILogger<ItemService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ItemResponse> CreateItemAsync(CreateItemRequest request)
        {
            try
            {
                var item = new Item 
                { 
                    Name = request.Name,
                    Price = request.Price,
                    Description = request.Description
                };

                var created = await _repository.AddAsync(item);
                return MapToResponse(created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating item");
                throw;
            }
        }

        public async Task<ItemResponse> GetItemAsync(string id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null)
                throw new KeyNotFoundException($"Item {id} not found");
            
            return MapToResponse(item);
        }

        public async Task<IEnumerable<ItemResponse>> ListItemsAsync()
        {
            var items = await _repository.GetAllAsync();
            return items.Select(MapToResponse);
        }

        private ItemResponse MapToResponse(Item item) => new()
        {
            Id = item.Id,
            Name = item.Name,
            Price = item.Price,
            Description = item.Description,
            CreatedAt = item.CreatedAt
        };
    }
}
