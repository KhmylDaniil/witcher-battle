using System;
using System.Collections.Generic;
using System.Linq;
using Wastelands.Core.EfDataAccess.Entities;

namespace Wastelands.Core.EfDataAccess.Contracts
{
	public interface IBaseRepository<TEntity> : IBaseReadRepository<TEntity> where TEntity : Entity
	{
		Task CreateAsync(TEntity entity);

		Task CreateRangeAsync(IEnumerable<TEntity> entities);

		Task UpdateAsync(TEntity entity);

		Task UpdateRangeAsync(IEnumerable<TEntity> entities);

		Task DeleteAsync(TEntity entity);

		Task DeleteRangeAsync(IEnumerable<TEntity> entities);

		Task UpdatePropertyForRangeAsync<TValue>(
			IEnumerable<TEntity> entities,
			string propertyName,
			TValue value);
	}
}
