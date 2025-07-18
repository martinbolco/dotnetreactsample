using Microsoft.EntityFrameworkCore;
using server_api.Data;
using server_api.Models;
using server_api.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server_api.Tests
{
	public class GenericRepositoryTests
	{
		private AppDbContext GetInMemoryDbContext()
		{
			var options = new DbContextOptionsBuilder<AppDbContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;

			return new AppDbContext(options);
		}

		[Fact]
		public async Task AddAsync_AddsEntityToDatabase()
		{
			// Arrange
			var context = GetInMemoryDbContext();
			var repository = new GenericRepository<Product>(context);
			var product = new Product { Name = "Test Product", Description = "test", Price = 10 };

			// Act
			await repository.AddAsync(product);
			await context.SaveChangesAsync();

			// Assert
			var allProducts = await repository.GetAllAsync();
			Assert.Single(allProducts);
			Assert.Equal("Test Product", allProducts.First().Name);
		}

		[Fact]
		public async Task GetByIdAsync_ReturnsCorrectEntity()
		{
			var context = GetInMemoryDbContext();
			var product = new Product { Name = "Test Product", Description = "test", Price = 10 };
			context.Products.Add(product);
			await context.SaveChangesAsync();

			var repository = new GenericRepository<Product>(context);
			var result = await repository.GetByIdAsync(product.Id);

			Assert.NotNull(result);
			Assert.Equal("Test Product", result?.Name);
		}

		[Fact]
		public async Task Update_UpdatesEntityInDatabase()
		{
			var context = GetInMemoryDbContext();
			var product = new Product {Name = "Old Name", Description = "test", Price = 10 };
			context.Products.Add(product);
			await context.SaveChangesAsync();

			var repository = new GenericRepository<Product>(context);
			product.Name = "Updated Name";
			repository.Update(product);
			await context.SaveChangesAsync();

			var updatedProduct = await repository.GetByIdAsync(product.Id);
			Assert.Equal("Updated Name", updatedProduct?.Name);
		}

		[Fact]
		public async Task Remove_DeletesEntityFromDatabase()
		{
			var context = GetInMemoryDbContext();
			var product = new Product {Name = "To Be Deleted", Description = "test", Price = 10 };
			context.Products.Add(product);
			await context.SaveChangesAsync();

			var repository = new GenericRepository<Product>(context);
			repository.Remove(product);
			await context.SaveChangesAsync();

			var result = await repository.GetAllAsync();
			Assert.Empty(result);
		}
	}
}
