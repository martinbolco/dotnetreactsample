using server_api.Repositories;

namespace server_api.Data
{
	public interface IUnitOfWork<T> : IDisposable where T : class
	{
		IGenericRepository<T> Repository { get; }
		Task<int> CompleteAsync(); // commits changes
	}
}
