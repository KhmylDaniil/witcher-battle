using Microsoft.EntityFrameworkCore;
using Wastelands.Core.Contracts.Contracts;
using Wastelands.Core.Contracts.Models;
using Wastelands.Core.EfDataAccess.Contracts;
using Wastelands.Core.EfDataAccess.Models;
using Wastelands.EfDataAccess.Extensions;
using Wastelands.EfDataAccess.Repositories;
using Wastelands.Service.Application.Contracts.Repositories;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Infrastructure.Repositories
{
	// GM-only ресурс, как BodyTemplateRepository: карта видна только создателю игры, к которой относится.
	// Когда карты начнут использоваться в бою, игрокам понадобится отдельный путь доступа (через бой).
	public class BattleMapRepository : BaseRepository<BattleMap>, IBattleMapRepository
	{
		private readonly DbContext _dbContext;
		private readonly IUserContext _userContext;

		public BattleMapRepository(DbContext context, IUserContext userContext) : base(context)
		{
			_dbContext = context;
			_userContext = userContext;
		}

		public async Task<PagedList<BattleMap>> GetPagedWithoutHexesAsync(PagedRequest request, IFilter<BattleMap>? filter)
		{
			return await GetQuery()
				.AsNoTracking()
				.ApplyFiltering(filter)
				.ApplyOrdering(request)
				.ApplyPagingAsync(request);
		}

		public async Task SaveTrackedChangesAsync()
		{
			await _dbContext.SaveChangesAsync();
		}

		protected override IQueryable<BattleMap> GetQuery()
		{
			var currentUserId = _userContext.CurrentUserId;

			return base.GetQuery()
				.Where(m => _dbContext.Set<Game>().Any(g => g.Id == m.GameId && g.CreatedByUserId == currentUserId));
		}

		protected override IQueryable<BattleMap> IncludeRelatedEntities(IQueryable<BattleMap> query)
		{
			return query.Include(x => x.Hexes);
		}
	}
}
