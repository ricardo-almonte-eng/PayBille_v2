using Microsoft.AspNetCore.Mvc;
using PayBille.Services;

namespace PayBille.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _service;

        public ItemsController(IItemService service)
        {
            _service = service;
        }

        /// <summary>
        /// Create a new item
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ItemResponse>> CreateItem([FromBody] CreateItemRequest request)
        {
            var result = await _service.CreateItemAsync(request);
            return CreatedAtAction(nameof(GetItem), new { id = result.Id }, result);
        }

        /// <summary>
        /// Get item by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ItemResponse>> GetItem(string id)
        {
            try
            {
                var result = await _service.GetItemAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// List all items
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ItemResponse>>> ListItems()
        {
            var results = await _service.ListItemsAsync();
            return Ok(results);
        }
    }
}
