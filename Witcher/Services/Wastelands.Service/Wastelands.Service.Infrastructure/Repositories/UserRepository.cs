using Microsoft.EntityFrameworkCore;
using Wastelands.Core.Contracts.Constants;
using Wastelands.Core.Contracts.Enums;
using Wastelands.Core.Contracts.Exceptions.WebExceptions;
using Wastelands.EfDataAccess.Repositories;
using Wastelands.Service.Domain.Contracts.Repositories;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Infrastructure.Repositories
{
	public class UserRepository : BaseRepository<User>, IUserRepository
	{
		public UserRepository(DbContext context) : base(context)
		{
		}

		public async Task<User> GetByLoginAsync(string login)
		{
			return await _context.Set<User>().SingleOrDefaultAsync(x => x.Login == login)
				?? throw new BadRequestException(ErrorCode.LoginNotFound, ExceptionMessages.LoginNotFound);
		}
	}
}
