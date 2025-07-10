using Microsoft.EntityFrameworkCore;
using server_api.Data;
using server_api.Models;

namespace server_api.Repositories
{
	public class ProductRepository : GenericRepository<Product>, IProductRepository
	{
		public ProductRepository(AppDbContext context) : base(context) { }

		public async Task<IEnumerable<Product>> GetByNameAsync(string name)
		{
			return await _context.Products
				.Where(p => p.Name.Contains(name))
				.ToListAsync();
		}
	}
}
