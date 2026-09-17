using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Core.EfDataAccess.Entities;

namespace Wastelands.Service.Domain.Entities
{
	/// <summary>
	/// Одна строка лога боя — видна всем участникам боя (не только сторонам конкретной атаки).
	/// </summary>
	public class BattleLogEntry : Entity
	{
		public long BattleId { get; private set; }

		public string Message { get; private set; }

		public DateTime CreatedAt { get; private set; }

		private BattleLogEntry()
		{
		}

		public BattleLogEntry(long battleId, string message)
		{
			InvalidArgumentException.ThrowIfLessOrEqualToZero(battleId, nameof(battleId));
			InvalidArgumentException.ThrowIfNullOrEmpty(message, nameof(message));

			BattleId = battleId;
			Message = message;
			CreatedAt = DateTime.UtcNow;
		}
	}
}
