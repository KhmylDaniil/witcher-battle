using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using Wastelands.Core.EfDataAccess.Contracts;
using Wastelands.Core.EfDataAccess.Entities;

namespace Wastelands.EfDataAccess.Repositories
{
	public abstract class BaseRepository<TEntity> : BaseReadRepository<TEntity>, IBaseRepository<TEntity>
	where TEntity : Entity
	{
		protected BaseRepository(DbContext context) : base(context)
		{
		}

		public async Task CreateAsync(TEntity entity)
		{
			await _context.Set<TEntity>().AddAsync(entity);
			await _context.SaveChangesAsync();
		}

		public async Task CreateRangeAsync(IEnumerable<TEntity> entities)
		{
			await _context.Set<TEntity>().AddRangeAsync(entities);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(TEntity entity)
		{
			_context.Set<TEntity>().Remove(entity);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteRangeAsync(IEnumerable<TEntity> entities)
		{
			if (!entities.Any())
			{
				return;
			}

			var ids = entities.Select(x => x.Id).ToList();

			await GetQuery()
				.Where(x => ids.Contains(x.Id))
				.ExecuteDeleteAsync();
		}

		public async Task UpdateAsync(TEntity entity)
		{
			_context.Set<TEntity>().Update(entity);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateRangeAsync(IEnumerable<TEntity> entities)
		{
			_context.Set<TEntity>().UpdateRange(entities);
			await _context.SaveChangesAsync();
		}

		public async Task UpdatePropertyForRangeAsync<TValue>(
			IEnumerable<TEntity> entities,
			string propertyName,
			TValue value)
		{
			if (!entities.Any())
			{
				return;
			}

			var ids = entities.Select(x => x.Id).ToList();
			var expression = GetUpdateExpression(GetPropertyExpression<TValue>(propertyName), value);

			await GetQuery()
				.Where(x => ids.Contains(x.Id))
				.ExecuteUpdateAsync(expression);
		}

		private static Expression<Func<TEntity, TValue>> GetPropertyExpression<TValue>(string propertyName)
		{
			var arg = Expression.Parameter(typeof(TEntity), "x");
			var property = Expression.Property(arg, propertyName);

			return Expression.Lambda<Func<TEntity, TValue>>(property, arg);
		}

		private static Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> GetUpdateExpression<TProperty>(
			Expression<Func<TEntity, TProperty>> property, TProperty value)
		{
			var setParameter = Expression.Parameter(typeof(SetPropertyCalls<TEntity>), "x");

			var method = typeof(SetPropertyCalls<TEntity>)
				.GetMethods()
				.Last(x => x.Name == nameof(SetPropertyCalls<TEntity>.SetProperty));

			var call = Expression.Call(setParameter, method.MakeGenericMethod(typeof(TProperty)), property, Expression.Constant(value));

			return Expression.Lambda<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>>(call, setParameter);
		}
	}
}
