using System.Linq.Expressions;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Domain.Models.Filters
{
	public class UserGameFilter : BaseFilter<UserGame>
	{
		public long? UserId { get; set; }

		public long? GameId { get; set; }

		public override Expression<Func<UserGame, bool>> GetFilterExpression()
		{
			return entity =>
			(Id == null || entity.Id == Id) &&
			(UserId == null || entity.UserId == UserId) &&
			(GameId == null || entity.GameId == GameId);
		}
	}
}
