using server_api.Models;

namespace server_api.Services
{
	public interface IProductService
	{
		Task<IEnumerable<Product>> GetProductsAsync();
		Task<Product?> GetProductAsync(int id);
		Task<Product> CreateProductAsync(Product product);
		Task<bool> UpdateProductAsync(int id, Product updatedProduct);
		Task<bool> DeleteProductAsync(int id);
	}
}
