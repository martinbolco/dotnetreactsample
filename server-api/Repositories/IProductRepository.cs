using server_api.Models;

namespace server_api.Repositories
{
	public interface IProductRepository : IGenericRepository<Product>
	{
		Task<IEnumerable<Product>> GetByNameAsync(string name);
	}
}
