using System.Linq.Expressions;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Application.Models.Filters
{
	public class BattleFilter : BaseFilter<Battle>
	{
		public long? GameId { get; set; }

		public override Expression<Func<Battle, bool>> GetFilterExpression()
		{
			return entity =>
			(Id == null || entity.Id == Id) &&
			(GameId == null || entity.GameId == GameId);
		}
	}
}
