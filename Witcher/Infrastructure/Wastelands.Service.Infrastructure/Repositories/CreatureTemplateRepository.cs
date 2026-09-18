using Microsoft.EntityFrameworkCore;
using Wastelands.Core.Contracts.Contracts;
using Wastelands.EfDataAccess.Repositories;
using Wastelands.Service.Application.Contracts.Repositories;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Infrastructure.Repositories
{
	// GM-only ресурс — тот же принцип скоупинга, что и BodyTemplateRepository.
	public class CreatureTemplateRepository : BaseRepository<CreatureTemplate>, ICreatureTemplateRepository
	{
		private readonly DbContext _dbContext;
		private readonly IUserContext _userContext;

		public CreatureTemplateRepository(DbContext context, IUserContext userContext) : base(context)
		{
			_dbContext = context;
			_userContext = userContext;
		}

		protected override IQueryable<CreatureTemplate> GetQuery()
		{
			var currentUserId = _userContext.CurrentUserId;

			return base.GetQuery()
				.Where(ct => _dbContext.Set<Game>().Any(g => g.Id == ct.GameId && g.CreatedByUserId == currentUserId));
		}

		protected override IQueryable<CreatureTemplate> IncludeRelatedEntities(IQueryable<CreatureTemplate> query)
		{
			return query
				.Include(x => x.Parts)
				.Include(x => x.Abilities).ThenInclude(a => a.AppliedConditions)
				.Include(x => x.Abilities).ThenInclude(a => a.DefensiveSkills);
		}

		public async Task<CreatureTemplate?> GetByIdUnscopedAsync(long id)
		{
			return await IncludeRelatedEntities(_dbContext.Set<CreatureTemplate>())
				.FirstOrDefaultAsync(x => x.Id == id);
		}
	}
}
