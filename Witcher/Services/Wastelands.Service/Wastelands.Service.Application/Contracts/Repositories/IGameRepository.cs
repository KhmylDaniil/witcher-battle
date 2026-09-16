using Wastelands.Core.EfDataAccess.Contracts;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Application.Contracts.Repositories
{
	public interface IGameRepository : IBaseRepository<Game>
	{
		Task<List<Game>> GetMyGamesAsync(long userId);
	}
}
