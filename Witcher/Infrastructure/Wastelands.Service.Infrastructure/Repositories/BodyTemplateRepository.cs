using Microsoft.EntityFrameworkCore;
using Wastelands.Core.Contracts.Contracts;
using Wastelands.EfDataAccess.Repositories;
using Wastelands.Service.Application.Contracts.Repositories;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Infrastructure.Repositories
{
	// GM-only ресурс: BodyTemplate виден и доступен только создателю игры, к которой он относится (та же
	// идея, что и CharacterRepository.GetQuery(), только владелец определяется через связанную Game, а не
	// напрямую по UserId, отсюда — join через DbContext, а не простой Where).
	public class BodyTemplateRepository : BaseRepository<BodyTemplate>, IBodyTemplateRepository
	{
		private readonly DbContext _dbContext;
		private readonly IUserContext _userContext;

		public BodyTemplateRepository(DbContext context, IUserContext userContext) : base(context)
		{
			_dbContext = context;
			_userContext = userContext;
		}

		protected override IQueryable<BodyTemplate> GetQuery()
		{
			var currentUserId = _userContext.CurrentUserId;

			return base.GetQuery()
				.Where(bt => _dbContext.Set<Game>().Any(g => g.Id == bt.GameId && g.CreatedByUserId == currentUserId));
		}

		protected override IQueryable<BodyTemplate> IncludeRelatedEntities(IQueryable<BodyTemplate> query)
		{
			return query.Include(x => x.Parts);
		}
	}
}
