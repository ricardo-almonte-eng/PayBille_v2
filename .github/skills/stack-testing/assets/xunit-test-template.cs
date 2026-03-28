using Xunit;
using Moq;
using PayBille.Services;
using PayBille.Data.Repositories;

namespace PayBille.Tests.Services
{
    public class ItemServiceTests
    {
        private readonly Mock<IItemRepository> _mockRepository;
        private readonly ItemService _service;

        public ItemServiceTests()
        {
            _mockRepository = new Mock<IItemRepository>();
            _service = new ItemService(_mockRepository.Object, new MockLogger());
        }

        [Fact]
        public async Task CreateItemAsync_WithValidRequest_ReturnsItem()
        {
            // Arrange
            var request = new CreateItemRequest 
            { 
                Name = "Test Item",
                Price = 99.99m,
                Description = "Test Description"
            };

            var expectedItem = new Item 
            { 
                Id = "123",
                Name = request.Name,
                Price = request.Price
            };

            _mockRepository.Setup(r => r.AddAsync(It.IsAny<Item>()))
                .ReturnsAsync(expectedItem);

            // Act
            var result = await _service.CreateItemAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("123", result.Id);
            Assert.Equal("Test Item", result.Name);
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<Item>()), Times.Once);
        }

        [Fact]
        public async Task GetItemAsync_WithInvalidId_ThrowsKeyNotFoundException()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((Item)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _service.GetItemAsync("invalid-id")
            );
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task CreateItemAsync_WithInvalidName_ThrowsArgumentException(string invalidName)
        {
            // Arrange
            var request = new CreateItemRequest 
            { 
                Name = invalidName,
                Price = 99.99m
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(
                () => _service.CreateItemAsync(request)
            );
        }
    }
}
