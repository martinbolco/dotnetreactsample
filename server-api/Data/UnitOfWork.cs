using server_api.Repositories;

namespace server_api.Data
{
	public class UnitOfWork<T> : IUnitOfWork<T> where T : class
	{
		private readonly AppDbContext _context;
		public IGenericRepository<T> Repository { get; }

		public UnitOfWork(AppDbContext context)
		{
			_context = context;
			Repository = new GenericRepository<T>(_context);
		}

		public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

		public void Dispose() => _context.Dispose();
	}
}
