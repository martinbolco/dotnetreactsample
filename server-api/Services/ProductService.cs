using Microsoft.EntityFrameworkCore;
using server_api.Data;
using server_api.Models;
using server_api.Repositories;

namespace server_api.Services
{
	public class ProductService : IProductService
	{
		private readonly IUnitOfWork<Product> _unitOfWork;

		public ProductService(IUnitOfWork<Product> unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<IEnumerable<Product>> GetProductsAsync()
		{
			return await _unitOfWork.Repository.GetAllAsync();
		}

		public async Task<Product?> GetProductAsync(int id)
		{
			return await _unitOfWork.Repository.GetByIdAsync(id);
		}

		public async Task<Product> CreateProductAsync(Product product)
		{
			await _unitOfWork.Repository.AddAsync(product);
			await _unitOfWork.CompleteAsync();
			return product;
		}

		public async Task<bool> UpdateProductAsync(int id, Product updatedProduct)
		{
			if (id != updatedProduct.Id)
				return false;

			_unitOfWork.Repository.Update(updatedProduct);
			await _unitOfWork.CompleteAsync();
			return true;
		}

		public async Task<bool> DeleteProductAsync(int id)
		{
			var product = await _unitOfWork.Repository.GetByIdAsync(id);
			if (product == null)
				return false;

			_unitOfWork.Repository.Remove(product);
			await _unitOfWork.CompleteAsync();
			return true;
		}
	}
}
