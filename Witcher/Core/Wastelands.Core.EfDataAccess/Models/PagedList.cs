using Wastelands.Core.EfDataAccess.Entities;

namespace Wastelands.Core.EfDataAccess.Models
{
	public class PagedList<TEntity>(long totalCount, List<TEntity> entities) where TEntity : Entity
	{
		public long TotalCount { get; } = totalCount;

		public List<TEntity> Entities { get; } = entities;
	}
}
