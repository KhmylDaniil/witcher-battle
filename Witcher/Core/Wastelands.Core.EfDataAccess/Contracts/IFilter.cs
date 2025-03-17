using System.Linq.Expressions;
using Wastelands.Core.EfDataAccess.Entities;

namespace Wastelands.Core.EfDataAccess.Contracts
{
	public interface IFilter<TEntity> where TEntity : Entity
	{
		Expression<Func<TEntity, bool>> GetFilterExpression();
	}
}
