using Microsoft.EntityFrameworkCore;
using Wastelands.Core.Contracts.Contracts;
using Wastelands.EfDataAccess.Repositories;
using Wastelands.Service.Application.Contracts.Repositories;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Infrastructure.Repositories
{
	// GM-only ресурс — тот же принцип скоупинга, что и CreatureTemplateRepository/BodyTemplateRepository.
	public class ItemTemplateRepository : BaseRepository<ItemTemplate>, IItemTemplateRepository
	{
		private readonly DbContext _dbContext;
		private readonly IUserContext _userContext;

		public ItemTemplateRepository(DbContext context, IUserContext userContext) : base(context)
		{
			_dbContext = context;
			_userContext = userContext;
		}

		protected override IQueryable<ItemTemplate> GetQuery()
		{
			var currentUserId = _userContext.CurrentUserId;

			return base.GetQuery()
				.Where(it => _dbContext.Set<Game>().Any(g => g.Id == it.GameId && g.CreatedByUserId == currentUserId));
		}

		protected override IQueryable<ItemTemplate> IncludeRelatedEntities(IQueryable<ItemTemplate> query)
		{
			return query.Include(x => x.AppliedConditions).Include(x => x.ArmorParts);
		}
	}
}
