using System.Linq.Expressions;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Domain.Models.Filters
{
	public class GameJoinRequestFilter : BaseFilter<GameJoinRequest>
	{
		public long? UserId { get; set; }

		public long? GameId { get; set; }

		public GameJoinRequestStatus? Status { get; set; }

		public override Expression<Func<GameJoinRequest, bool>> GetFilterExpression()
		{
			return entity =>
			(Id == null || entity.Id == Id) &&
			(UserId == null || entity.UserId == UserId) &&
			(GameId == null || entity.GameId == GameId) &&
			(Status == null || entity.Status == Status);
		}
	}
}
