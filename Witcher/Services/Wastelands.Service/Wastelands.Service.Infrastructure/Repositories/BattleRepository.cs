using Microsoft.EntityFrameworkCore;
using Wastelands.Core.Contracts.Contracts;
using Wastelands.EfDataAccess.Repositories;
using Wastelands.Service.Application.Contracts.Repositories;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Infrastructure.Repositories
{
	// Мастер игры видит все свои бои всегда; обычный игрок — только после начала боя (Status ==
	// InProgress) и только если среди его персонажей есть участник этого боя. До старта боя или для
	// стороннего пользователя запрос возвращает пусто, т.е. 404, а не 403 — не палим существование.
	public class BattleRepository : BaseRepository<Battle>, IBattleRepository
	{
		private readonly DbContext _dbContext;
		private readonly IUserContext _userContext;

		public BattleRepository(DbContext context, IUserContext userContext) : base(context)
		{
			_dbContext = context;
			_userContext = userContext;
		}

		protected override IQueryable<Battle> GetQuery()
		{
			var currentUserId = _userContext.CurrentUserId;

			return base.GetQuery()
				.Where(b =>
					_dbContext.Set<Game>().Any(g => g.Id == b.GameId && g.CreatedByUserId == currentUserId) ||
					(b.Status == BattleStatus.InProgress &&
						_dbContext.Set<BattleCharacter>().Any(bc => bc.BattleId == b.Id &&
							_dbContext.Set<Character>().Any(c => c.Id == bc.CharacterId && c.UserId == currentUserId))));
		}

		protected override IQueryable<Battle> IncludeRelatedEntities(IQueryable<Battle> query)
		{
			return query
				.Include(x => x.Creatures)
				.Include(x => x.Characters).ThenInclude(bc => bc.Character)
				.Include(x => x.Attack)
				.Include(x => x.LogEntries);
		}
	}
}
