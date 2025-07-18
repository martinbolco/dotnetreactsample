using Microsoft.EntityFrameworkCore;
using Moq;
using server_api.Data;
using server_api.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server_api.Tests
{
	public class UnitOfWorkTests
	{
		[Fact]
		public void Constructor_ShouldInitializeRepository()
		{
			// Arrange
			var dbContextMock = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());

			// Act
			var unitOfWork = new UnitOfWork<SampleEntity>(dbContextMock.Object);

			// Assert
			Assert.NotNull(unitOfWork.Repository);
			Assert.IsAssignableFrom<IGenericRepository<SampleEntity>>(unitOfWork.Repository);
		}

		[Fact]
		public async Task CompleteAsync_ShouldCallSaveChangesAsync()
		{
			// Arrange
			var dbContextMock = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
			dbContextMock.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

			var unitOfWork = new UnitOfWork<SampleEntity>(dbContextMock.Object);

			// Act
			var result = await unitOfWork.CompleteAsync();

			// Assert
			Assert.Equal(1, result);
			dbContextMock.Verify(c => c.SaveChangesAsync(default), Times.Once);
		}

		[Fact]
		public void Dispose_ShouldCallContextDispose()
		{
			// Arrange
			var dbContextMock = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
			var unitOfWork = new UnitOfWork<SampleEntity>(dbContextMock.Object);

			// Act
			unitOfWork.Dispose();

			// Assert
			dbContextMock.Verify(c => c.Dispose(), Times.Once);
		}

		public class SampleEntity { public int Id { get; set; } }
	}
}
