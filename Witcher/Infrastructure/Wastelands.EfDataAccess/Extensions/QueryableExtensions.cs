using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Wastelands.Core.Contracts.Contracts;
using Wastelands.Core.Contracts.Enums;
using Wastelands.Core.EfDataAccess.Contracts;
using Wastelands.Core.EfDataAccess.Entities;
using Wastelands.Core.EfDataAccess.Models;

namespace Wastelands.EfDataAccess.Extensions
{
	public static class QueryableExtensions
	{
		public static IQueryable<TEntity> ApplyOrdering<TEntity>(this IQueryable<TEntity> query, IOrderParams ordering)
			where TEntity : Entity
		{
			var param = Expression.Parameter(typeof(TEntity), "x");
			var parts = ordering.OrderBy.Split('.');

			Expression parent = param;

			foreach (var part in parts)
			{
				parent = Expression.Property(parent, part);
			}

			var conversion = Expression.Convert(parent, typeof(object));
			var sortExpression = Expression.Lambda<Func<TEntity, object>>(conversion, param);

			if (parts[0] != nameof(Entity.Id))
			{
				return ordering.OrderDirection == OrderDirection.Descending ?
					query.OrderByDescending(sortExpression).ThenByDescending(x => x.Id) :
					query.OrderBy(sortExpression).ThenBy(x => x.Id);
			}

			return ordering.OrderDirection == OrderDirection.Descending ?
				query.OrderByDescending(sortExpression) :
				query.OrderBy(sortExpression);
		}

		public static IQueryable<TEntity> ApplyFiltering<TEntity>(
			this IQueryable<TEntity> query,
			IFilter<TEntity>? filter)
			where TEntity : Entity
		{
			if (filter is null)
			{
				return query;
			}

			return query.Where(filter.GetFilterExpression());
		}

		public static async Task<PagedList<TEntity>> ApplyPagingAsync<TEntity>(
			this IQueryable<TEntity> query,
			IPagingParams paging)
			where TEntity : Entity
		{
			var count = await query.LongCountAsync();
			var items = await query
				.Skip((paging.PageNumber - 1) * paging.PageSize)
				.Take(paging.PageSize)
				.ToListAsync();

			return new PagedList<TEntity>(count, items);
		}
	}
}
