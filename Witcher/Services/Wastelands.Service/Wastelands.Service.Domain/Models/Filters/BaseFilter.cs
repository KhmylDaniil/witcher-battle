using System.Linq.Expressions;
using Wastelands.Core.EfDataAccess.Contracts;
using Wastelands.Core.EfDataAccess.Entities;

namespace Wastelands.Service.Domain.Models.Filters
{
	public abstract class BaseFilter<TEntity> : IFilter<TEntity> where TEntity : Entity
	{
		/// <summary>
		/// Идентификатор записи в хранилище.
		/// </summary>
		public long? Id { get; set; }

		public abstract Expression<Func<TEntity, bool>> GetFilterExpression();
	}
}
