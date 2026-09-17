using Microsoft.EntityFrameworkCore;
using Wastelands.Core.Contracts.Contracts;
using Wastelands.EfDataAccess.Repositories;
using Wastelands.Service.Application.Contracts.Repositories;
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

		protected override IQueryable<Character> IncludeRelatedEntities(IQueryable<Character> query)
		{
			return query
				.Include(x => x.Abilities).ThenInclude(a => a.AppliedConditions)
				.Include(x => x.Abilities).ThenInclude(a => a.DefensiveSkills);
		}

		public async Task<List<Character>> GetCharactersByGameIdAsync(long gameId)
		{
			return await _context.Set<Character>().Where(x => x.GameId == gameId).ToListAsync();
		}

		public async Task<Character?> GetByIdUnscopedAsync(long id)
		{
			return await IncludeRelatedEntities(_context.Set<Character>())
				.FirstOrDefaultAsync(x => x.Id == id);
		}
	}
}
