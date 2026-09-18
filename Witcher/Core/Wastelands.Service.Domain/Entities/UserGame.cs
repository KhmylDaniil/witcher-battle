using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Core.EfDataAccess.Entities;

namespace Wastelands.Service.Domain.Entities
{
	public class UserGame : Entity
	{
		public long UserId { get; private set; }

		public long GameId { get; private set; }

		private UserGame()
		{
		}

		public UserGame(long userId, long gameId)
		{
			InvalidArgumentException.ThrowIfLessOrEqualToZero(userId, nameof(userId));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(gameId, nameof(gameId));

			UserId = userId;
			GameId = gameId;
		}
	}
}
