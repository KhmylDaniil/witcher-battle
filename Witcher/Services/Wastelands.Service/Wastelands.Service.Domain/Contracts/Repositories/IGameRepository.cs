using Wastelands.Core.EfDataAccess.Contracts;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Domain.Contracts.Repositories
{
	public interface IGameRepository : IBaseRepository<Game>
	{
		Task<List<Game>> GetMyGamesAsync(long userId);
	}
}
