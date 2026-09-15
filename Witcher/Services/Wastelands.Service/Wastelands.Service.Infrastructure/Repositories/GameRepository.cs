using Microsoft.EntityFrameworkCore;
using Wastelands.EfDataAccess.Repositories;
using Wastelands.Service.Domain.Contracts.Repositories;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Infrastructure.Repositories
{
	public class GameRepository : BaseRepository<Game>, IGameRepository
	{
		private readonly DbContext _dbContext;

		public GameRepository(DbContext context) : base(context)
		{
			_dbContext = context;
		}

		public async Task<List<Game>> GetMyGamesAsync(long userId)
		{
			return await GetQuery()
				.Where(g => g.CreatedByUserId == userId || _dbContext.Set<UserGame>().Any(ug => ug.GameId == g.Id && ug.UserId == userId))
				.ToListAsync();
		}
	}
}
