using server_api.Repositories;

namespace server_api.Data
{
	public interface IUnitOfWork : IDisposable
	{
		IProductRepository Products { get; }
		Task<int> CompleteAsync(); // commits changes
	}
}
