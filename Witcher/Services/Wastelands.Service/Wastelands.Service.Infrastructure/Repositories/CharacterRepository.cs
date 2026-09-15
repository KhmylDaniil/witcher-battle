using Microsoft.EntityFrameworkCore;
using Wastelands.Core.Contracts.Contracts;
using Wastelands.EfDataAccess.Repositories;
using Wastelands.Service.Domain.Contracts.Repositories;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Infrastructure.Repositories
{
	public class CharacterRepository : BaseRepository<Character>, ICharacterRepository
	{
		private readonly IUserContext _userContext;

		public CharacterRepository(DbContext context, IUserContext userContext) : base(context)
		{
			_userContext = userContext;
		}

		protected override IQueryable<Character> GetQuery()
		{
			return base.GetQuery().Where(x => x.UserId == _userContext.CurrentUserId);
		}
	}
}
