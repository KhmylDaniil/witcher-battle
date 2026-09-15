using Microsoft.EntityFrameworkCore;
using Wastelands.EfDataAccess.Repositories;
using Wastelands.Service.Domain.Contracts.Repositories;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Infrastructure.Repositories
{
	public class GameJoinRequestRepository : BaseRepository<GameJoinRequest>, IGameJoinRequestRepository
	{
		public GameJoinRequestRepository(DbContext context) : base(context)
		{
		}
	}
}
