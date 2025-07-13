using Moq;
using server_api.Data;
using server_api.Models;
using server_api.Repositories;
using server_api.Services;

namespace server_api.Tests
{
	public class ProductServiceTests
	{
		private readonly Mock<IUnitOfWork<Product>> _unitOfWorkMock;
		private readonly Mock<IGenericRepository<Product>> _repoMock;
		private readonly ProductService _service;

		public ProductServiceTests()
		{
			_unitOfWorkMock = new Mock<IUnitOfWork<Product>>();
			_repoMock = new Mock<IGenericRepository<Product>>();
			_unitOfWorkMock.Setup(u => u.Repository).Returns(_repoMock.Object);
			_service = new ProductService(_unitOfWorkMock.Object);
		}

		[Fact]
        public async Task GetProductsAsync_ReturnsAllProducts()
        {
            // Arrange
            var expected = new List<Product> { new() { Id = 1, Name = "Test", Description = "Test Description" } };
            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(expected);

            // Act
            var result = await _service.GetProductsAsync();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetProductAsync_WithValidId_ReturnsProduct()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Test", Description = "Test Description" };
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

            // Act
            var result = await _service.GetProductAsync(1);

            // Assert
            Assert.Equal(product, result);
        }

        [Fact]
        public async Task CreateProductAsync_AddsProductAndSaves()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "New", Description = "Test Description" };

            // Act
            var result = await _service.CreateProductAsync(product);

            // Assert
            _repoMock.Verify(r => r.AddAsync(product), Times.Once);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
            Assert.Equal(product, result);
        }

        [Fact]
        public async Task UpdateProductAsync_WithMismatchedId_ReturnsFalse()
        {
            var updated = new Product { Id = 2, Name = "X", Description = "Test Description" };
            var result = await _service.UpdateProductAsync(1, updated);
            Assert.False(result);
        }

        [Fact]
        public async Task UpdateProductAsync_WithValidId_UpdatesProduct()
        {
            var updated = new Product { Id = 1, Name = "X", Description = "Test Description" };

            var result = await _service.UpdateProductAsync(1, updated);

            _repoMock.Verify(r => r.Update(updated), Times.Once);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteProductAsync_WithNonExistentProduct_ReturnsFalse()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Product)null!);

            var result = await _service.DeleteProductAsync(1);

            Assert.False(result);
        }

        [Fact]
        public async Task DeleteProductAsync_WithValidProduct_RemovesProduct()
        {
            var product = new Product { Id = 1, Name = "Test", Description = "Test Description" };
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

            var result = await _service.DeleteProductAsync(1);

            _repoMock.Verify(r => r.Remove(product), Times.Once);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
            Assert.True(result);
        }
	}
}