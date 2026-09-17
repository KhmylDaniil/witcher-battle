using Wastelands.Core.Contracts.Enums;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Core.EfDataAccess.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Domain.Entities
{
	public class Battle : Entity
	{
		public long GameId { get; private set; }

		public string Name { get; private set; }

		public BattleStatus Status { get; private set; } = BattleStatus.Draft;

		public int CurrentRound { get; private set; } = 1;

		/// <summary>Значение Initiative участника, чей сейчас ход. Null, пока бой не начат.</summary>
		public int? CurrentInitiative { get; private set; }

		public List<Creature> Creatures { get; private set; } = [];

		public List<BattleCharacter> Characters { get; private set; } = [];

		public BattleAttack? Attack { get; private set; }

		public List<BattleLogEntry> LogEntries { get; private set; } = [];

		private Battle()
		{
		}

		public Battle(long gameId, string name)
		{
			InvalidArgumentException.ThrowIfLessOrEqualToZero(gameId, nameof(gameId));
			InvalidArgumentException.ThrowIfNullOrEmpty(name, nameof(name));

			GameId = gameId;
			Name = name;
		}

		public void MarkStarted()
		{
			if (Status != BattleStatus.Draft)
			{
				throw new InvalidArgumentException(
					ErrorCode.BattleAlreadyStarted,
					"Бой уже начат.");
			}

			Status = BattleStatus.InProgress;
			CurrentInitiative = 1;
		}

		/// <summary>
		/// Передаёт ход следующему по инициативе участнику; при переходе через последнего — на
		/// первого и инкрементирует CurrentRound.
		/// </summary>
		public void AdvanceTurn()
		{
			var totalParticipants = Creatures.Count + Characters.Count;
			if (CurrentInitiative is null || totalParticipants == 0)
			{
				throw new InvalidArgumentException(ErrorCode.InvalidArgument, "Бой ещё не начат.");
			}

			if (CurrentInitiative == totalParticipants)
			{
				CurrentInitiative = 1;
				CurrentRound++;
			}
			else
			{
				CurrentInitiative++;
			}
		}

		public void AddLogEntry(string message)
		{
			LogEntries.Add(new BattleLogEntry(Id, message));
		}

		public void StartAttack(BattleAttack attack)
		{
			if (Attack is not null)
			{
				throw new InvalidArgumentException(ErrorCode.AttackAlreadyInProgress, "В бою уже есть незавершённая атака.");
			}

			Attack = attack;
		}

		public void ClearAttack()
		{
			Attack = null;
		}
	}
}
