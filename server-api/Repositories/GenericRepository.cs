
using Microsoft.EntityFrameworkCore;
using server_api.Data;

namespace server_api.Repositories
{
	public class GenericRepository<T> : IGenericRepository<T> where T : class
	{
		protected readonly AppDbContext _context;
		private readonly DbSet<T> _dbSet;

		public GenericRepository(AppDbContext context)
		{
			_context = context;
			_dbSet = _context.Set<T>();
		}

		public virtual async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();
		public virtual async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);
		public virtual async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
		public virtual void Update(T entity) => _dbSet.Update(entity);
		public virtual void Remove(T entity) => _dbSet.Remove(entity);
	}
}
