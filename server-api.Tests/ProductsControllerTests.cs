using Microsoft.AspNetCore.Mvc;
using Moq;
using server_api.Controllers;
using server_api.Models;
using server_api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server_api.Tests
{
	public class ProductsControllerTests
	{
		private readonly ProductsController _controller;
		private readonly Mock<IProductService> _productServiceMock;

		public ProductsControllerTests()
		{
			_productServiceMock = new Mock<IProductService>();
			_controller = new ProductsController(_productServiceMock.Object);
		}

		[Fact]
		public async Task GetProducts_ReturnsOkResult_WithListOfProducts()
		{
			// Arrange
			var products = new List<Product>
		{
			new Product { Id = 1, Name = "Test Product 1", Description = "test", Price = 10 },
			new Product { Id = 2, Name = "Test Product 2", Description = "test", Price = 10 }
		};
			_productServiceMock.Setup(s => s.GetProductsAsync()).ReturnsAsync(products);

			// Act
			var result = await _controller.GetProducts();

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result.Result);
			var returnProducts = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);
			Assert.Equal(2, ((List<Product>)returnProducts).Count);
		}

		[Fact]
		public async Task GetProduct_ReturnsNotFound_IfNull()
		{
			_productServiceMock.Setup(s => s.GetProductAsync(It.IsAny<int>())).ReturnsAsync((Product)null);

			var result = await _controller.GetProduct(1);

			Assert.IsType<NotFoundResult>(result.Result);
		}

		[Fact]
		public async Task CreateProduct_ReturnsCreatedAtAction()
		{
			var product = new Product { Id = 3, Name = "Created Product", Description = "test", Price = 10 };
			_productServiceMock.Setup(s => s.CreateProductAsync(It.IsAny<Product>())).ReturnsAsync(product);

			var result = await _controller.CreateProduct(product);

			var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
			Assert.Equal("GetProduct", createdResult.ActionName);
		}

		[Fact]
		public async Task UpdateProduct_ReturnsBadRequest_IfIdsMismatch()
		{
			var updatedProduct = new Product { Id = 2, Name = "Updated", Description = "test", Price = 10 };

			var result = await _controller.UpdateProduct(1, updatedProduct);

			Assert.IsType<BadRequestResult>(result.Result);
		}

		[Fact]
		public async Task DeleteProduct_ReturnsNoContent_IfSuccessful()
		{
			_productServiceMock.Setup(s => s.DeleteProductAsync(1)).ReturnsAsync(true);

			var result = await _controller.DeleteProduct(1);

			Assert.IsType<NoContentResult>(result);
		}

		[Fact]
		public async Task GetProduct_ReturnsOkResult_WithProduct()
		{
			var product = new Product { Id = 1, Name = "Test", Description = "desc", Price = 10 };
			_productServiceMock.Setup(s => s.GetProductAsync(1)).ReturnsAsync(product);

			var result = await _controller.GetProduct(1);

			var okResult = Assert.IsType<OkObjectResult>(result.Result);
			var returnProduct = Assert.IsType<Product>(okResult.Value);
			Assert.Equal(1, returnProduct.Id);
		}

		[Fact]
		public async Task UpdateProduct_ReturnsOk_WhenUpdateSucceeds()
		{
			var updatedProduct = new Product { Id = 1, Name = "Updated", Description = "desc", Price = 10 };
			_productServiceMock.Setup(s => s.UpdateProductAsync(1, updatedProduct)).ReturnsAsync(true);

			var result = await _controller.UpdateProduct(1, updatedProduct);

			var okResult = Assert.IsType<OkObjectResult>(result.Result);
			var returnProduct = Assert.IsType<Product>(okResult.Value);
			Assert.Equal(1, returnProduct.Id);
		}

		[Fact]
		public async Task UpdateProduct_ReturnsNotFound_WhenUpdateFails()
		{
			var updatedProduct = new Product { Id = 1, Name = "Updated", Description = "desc", Price = 10 };
			_productServiceMock.Setup(s => s.UpdateProductAsync(1, updatedProduct)).ReturnsAsync(false);

			var result = await _controller.UpdateProduct(1, updatedProduct);

			Assert.IsType<NotFoundResult>(result.Result);
		}

		[Fact]
		public async Task DeleteProduct_ReturnsNotFound_WhenDeleteFails()
		{
			_productServiceMock.Setup(s => s.DeleteProductAsync(1)).ReturnsAsync(false);

			var result = await _controller.DeleteProduct(1);

			Assert.IsType<NotFoundResult>(result);
		}
	}
}
