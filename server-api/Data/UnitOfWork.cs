using server_api.Repositories;

namespace server_api.Data
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly AppDbContext _context;
		public IProductRepository Products { get; }

		public UnitOfWork(AppDbContext context)
		{
			_context = context;
			Products = new ProductRepository(_context);
		}

		public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

		public void Dispose() => _context.Dispose();
	}
}
