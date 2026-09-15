using System.Linq.Expressions;
using Wastelands.Core.Contracts.Contracts;
using Wastelands.Core.Contracts.Models;
using Wastelands.Core.EfDataAccess.Entities;
using Wastelands.Core.EfDataAccess.Models;

namespace Wastelands.Core.EfDataAccess.Contracts
{
	public interface IBaseReadRepository<TEntity> where TEntity : Entity
	{
		Task<List<TEntity>> GetAllAsync();

		Task<PagedList<TEntity>> GetPagedAsync(PagedRequest request, IFilter<TEntity>? filter);

		Task<List<TEntity>> GetListByFilterAsync(IFilter<TEntity>? filter);

		Task<List<TEntity>> GetOrderedListByFilterAsync(IFilter<TEntity>? filter, IOrderParams orderParams);

		Task<TEntity?> GetSingleByFilterAsync(IFilter<TEntity>? filter);

		Task<TEntity?> GetByIdAsync(long id);

		/// <summary>
		/// Gets the entity from Db by identifier, not joining related entities.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		Task<TEntity?> GetByIdWithoutJoinsAsync(long id);

		Task<List<TEntity>> GetByIdsAsync(IEnumerable<long> ids);

		Task<long> CountAsync();

		Task<long> CountAsync(Expression<Func<TEntity, bool>> condition);

		Task<bool> AnyAsync();

		Task<bool> AnyAsync(Expression<Func<TEntity, bool>> condition);
	}
}
