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

		/// <summary>
		/// Удаляет существо из боя (например, когда его HP падает до нуля — см.
		/// BattleParticipants.RemoveDeadCreatures). Персонажи так не удаляются, только существа.
		/// Переиндексирует Initiative оставшихся участников в плотную последовательность 1..N (её
		/// требует AdvanceTurn — см. BattleInitiativeRoller) и сохраняет, чей сейчас ход: если
		/// удаляемое существо и было активным участником — ход передаётся дальше по прежнему порядку
		/// (через AdvanceTurn, включая переход раунда), иначе активный участник остаётся тем же, просто
		/// с новым номером инициативы.
		/// </summary>
		public void RemoveCreature(Creature creature)
		{
			if (!Creatures.Contains(creature))
			{
				return;
			}

			if (CurrentInitiative is null)
			{
				Creatures.Remove(creature);
				return;
			}

			var active = FindParticipantByInitiative(CurrentInitiative.Value);
			if (active is { Kind: ParticipantKind.Creature } a && a.RefId == creature.Id)
			{
				AdvanceTurn();
				active = FindParticipantByInitiative(CurrentInitiative!.Value);
			}

			Creatures.Remove(creature);
			RenumberInitiative();

			CurrentInitiative = Creatures.Count + Characters.Count == 0 ? null : FindInitiativeOf(active);
		}

		private (ParticipantKind Kind, long RefId)? FindParticipantByInitiative(int initiative)
		{
			var creature = Creatures.FirstOrDefault(c => c.Initiative == initiative);
			if (creature is not null)
			{
				return (ParticipantKind.Creature, creature.Id);
			}

			var character = Characters.FirstOrDefault(c => c.Initiative == initiative);
			return character is null ? null : (ParticipantKind.Character, character.CharacterId);
		}

		private int? FindInitiativeOf((ParticipantKind Kind, long RefId)? participant)
		{
			if (participant is not { } p)
			{
				return null;
			}

			return p.Kind == ParticipantKind.Creature
				? Creatures.FirstOrDefault(c => c.Id == p.RefId)?.Initiative
				: Characters.FirstOrDefault(c => c.CharacterId == p.RefId)?.Initiative;
		}

		/// <summary>Переиндексирует Initiative всех оставшихся участников в плотную последовательность 1..N, сохраняя относительный порядок.</summary>
		private void RenumberInitiative()
		{
			var ordered = Creatures
				.Select(c => (initiative: c.Initiative!.Value, apply: (Action<int>)(v => c.ReassignInitiative(v))))
				.Concat(Characters.Select(bc => (initiative: bc.Initiative!.Value, apply: (Action<int>)(v => bc.ReassignInitiative(v)))))
				.OrderBy(x => x.initiative)
				.ToList();

			for (var i = 0; i < ordered.Count; i++)
			{
				ordered[i].apply(i + 1);
			}
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
