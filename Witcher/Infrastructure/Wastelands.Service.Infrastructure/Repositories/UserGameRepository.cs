using Microsoft.EntityFrameworkCore;
using Wastelands.EfDataAccess.Repositories;
using Wastelands.Service.Application.Contracts.Repositories;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Infrastructure.Repositories
{
	public class UserGameRepository : BaseRepository<UserGame>, IUserGameRepository
	{
		public UserGameRepository(DbContext context) : base(context)
		{
		}
	}
}
