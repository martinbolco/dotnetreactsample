using Microsoft.EntityFrameworkCore;
using server_api.Data;
using server_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server_api.Tests
{
	public class AppDbContextTests
	{
		private DbContextOptions<AppDbContext> GetInMemoryOptions()
		{
			return new DbContextOptionsBuilder<AppDbContext>()
				.UseInMemoryDatabase(databaseName: "TestDatabase")
				.Options;
		}

		[Fact]
		public async Task CanInsertProductIntoDatabase()
		{
			// Arrange
			var options = GetInMemoryOptions();

			using (var context = new AppDbContext(options))
			{
				var product = new Product { Id = 1, Name = "Test Product", Description = "test", Price = 10 };
				context.Products.Add(product);
				await context.SaveChangesAsync();
			}

			// Act
			using (var context = new AppDbContext(options))
			{
				var productFromDb = await context.Products.FindAsync(1);

				// Assert
				Assert.NotNull(productFromDb);
				Assert.Equal("Test Product", productFromDb.Name);
			}
		}
	}
}
