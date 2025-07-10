using Microsoft.EntityFrameworkCore;
using server_api.Data;
using server_api.Models;

namespace server_api.Services
{
	public class ProductService : IProductService
	{
		private readonly IUnitOfWork _unitOfWork;

		public ProductService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<IEnumerable<Product>> GetProductsAsync()
		{
			return await _unitOfWork.Products.GetAllAsync();
		}

		public async Task<Product?> GetProductAsync(int id)
		{
			return await _unitOfWork.Products.GetByIdAsync(id);
		}

		public async Task<Product> CreateProductAsync(Product product)
		{
			await _unitOfWork.Products.AddAsync(product);
			await _unitOfWork.CompleteAsync();
			return product;
		}

		public async Task<bool> UpdateProductAsync(int id, Product updatedProduct)
		{
			if (id != updatedProduct.Id)
				return false;

			_unitOfWork.Products.Update(updatedProduct);
			await _unitOfWork.CompleteAsync();
			return true;
		}

		public async Task<bool> DeleteProductAsync(int id)
		{
			var product = await _unitOfWork.Products.GetByIdAsync(id);
			if (product == null)
				return false;

			_unitOfWork.Products.Remove(product);
			await _unitOfWork.CompleteAsync();
			return true;
		}
	}
}
