using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Wastelands.Core.Contracts.Contracts;
using Wastelands.Core.Contracts.Models;
using Wastelands.Core.EfDataAccess.Contracts;
using Wastelands.Core.EfDataAccess.Entities;
using Wastelands.Core.EfDataAccess.Models;
using Wastelands.EfDataAccess.Extensions;

namespace Wastelands.EfDataAccess.Repositories
{
	public abstract class BaseReadRepository<TEntity> : IBaseReadRepository<TEntity> where TEntity : Entity
	{
		protected readonly DbContext _context;

		protected BaseReadRepository(DbContext context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public async Task<List<TEntity>> GetAllAsync()
		{
			return await IncludeRelatedEntities(GetQuery())
				.AsNoTracking()
				.ToListAsync();
		}

		public async Task<PagedList<TEntity>> GetPagedAsync(PagedRequest request, IFilter<TEntity>? filter)
		{
			return await IncludeRelatedEntities(GetQuery())
				.AsNoTracking()
				.ApplyFiltering(filter)
				.ApplyOrdering(request)
				.ApplyPagingAsync(request);
		}

		public async Task<List<TEntity>> GetListByFilterAsync(IFilter<TEntity>? filter)
		{
			return await IncludeRelatedEntities(GetQuery())
				.ApplyFiltering(filter)
				.ToListAsync();
		}

		public async Task<List<TEntity>> GetOrderedListByFilterAsync(IFilter<TEntity>? filter, IOrderParams orderParams)
		{
			return await IncludeRelatedEntities(GetQuery())
				.ApplyFiltering(filter)
				.ApplyOrdering(orderParams)
				.ToListAsync();
		}

		public async Task<TEntity?> GetSingleByFilterAsync(IFilter<TEntity>? filter)
		{
			return await IncludeRelatedEntities(GetQuery())
				.ApplyFiltering(filter)
				.FirstOrDefaultAsync();
		}

		public async Task<TEntity?> GetByIdAsync(long id)
		{
			return await IncludeRelatedEntities(GetQuery()).FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<TEntity?> GetByIdWithoutJoinsAsync(long id)
		{
			return await GetQuery().FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<List<TEntity>> GetByIdsAsync(IEnumerable<long> ids)
		{
			return await IncludeRelatedEntities(GetQuery())
				.Where(x => ids.Contains(x.Id))
				.ToListAsync();
		}

		public async Task<long> CountAsync()
		{
			return await GetQuery().CountAsync();
		}

		public async Task<long> CountAsync(Expression<Func<TEntity, bool>> condition)
		{
			return await GetQuery().CountAsync(condition);
		}

		public async Task<bool> AnyAsync()
		{
			return await GetQuery().AnyAsync();
		}

		public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> condition)
		{
			return await GetQuery().AnyAsync(condition);
		}

		protected virtual IQueryable<TEntity> GetQuery()
		{
			return _context.Set<TEntity>().AsQueryable();
		}

		protected virtual IQueryable<TEntity> IncludeRelatedEntities(IQueryable<TEntity> query)
		{
			return query;
		}
	}
}
