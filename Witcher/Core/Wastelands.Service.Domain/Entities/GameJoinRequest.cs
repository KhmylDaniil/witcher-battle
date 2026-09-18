using Wastelands.Core.Contracts.Enums;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Core.EfDataAccess.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Domain.Entities
{
	public class GameJoinRequest : Entity
	{
		public long UserId { get; private set; }

		public long GameId { get; private set; }

		public GameJoinRequestStatus Status { get; private set; }

		private GameJoinRequest()
		{
		}

		public GameJoinRequest(long userId, long gameId)
		{
			InvalidArgumentException.ThrowIfLessOrEqualToZero(userId, nameof(userId));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(gameId, nameof(gameId));

			UserId = userId;
			GameId = gameId;
			Status = GameJoinRequestStatus.Pending;
		}

		public void Accept()
		{
			ThrowIfNotPending();
			Status = GameJoinRequestStatus.Accepted;
		}

		public void Decline()
		{
			ThrowIfNotPending();
			Status = GameJoinRequestStatus.Declined;
		}

		private void ThrowIfNotPending()
		{
			if (Status != GameJoinRequestStatus.Pending)
			{
				throw new InvalidArgumentException(ErrorCode.GameJoinRequestNotPending, "Заявка уже обработана.");
			}
		}
	}
}
