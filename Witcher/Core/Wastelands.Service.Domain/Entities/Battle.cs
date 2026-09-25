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

		/// <summary>Гексагональная карта, на которой идёт бой (см. <see cref="AttachMap"/>). Null — бой без карты.</summary>
		public long? BattleMapId { get; private set; }

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

			// Окно дополнительного действия (см. BattleCharacter.HasActedThisTurn) действует только до
			// конца текущего хода того же персонажа — при любой передаче хода оно закрывается.
			foreach (var character in Characters)
			{
				character.ResetTurnState();
			}
		}

		/// <summary>
		/// Добавляет существо в бой. До начала боя инициатива не выставляется (её разыгрывает старт боя),
		/// а в уже идущем бою — см. <see cref="AssignLateInitiative"/>.
		/// </summary>
		/// <returns>true — бой шёл без участников, и добавленный сразу становится активным (вызывающему
		/// нужно обработать начало его хода).</returns>
		public bool AddCreature(Creature creature)
		{
			Creatures.Add(creature);
			return AssignLateInitiative(creature.SetInitiative);
		}

		/// <inheritdoc cref="AddCreature"/>
		public bool AddCharacter(BattleCharacter character)
		{
			Characters.Add(character);
			return AssignLateInitiative(character.SetInitiative);
		}

		/// <summary>
		/// Участник, вступивший в уже идущий бой, инициативу не бросает: он встаёт в конец очереди
		/// (номер N+1 при плотной нумерации 1..N — её поддерживает RenumberInitiative), поэтому
		/// несколько поздних участников ходят в порядке добавления. Если все прежние участники выбыли
		/// (CurrentInitiative == null), ход сразу переходит к новому.
		/// </summary>
		private bool AssignLateInitiative(Action<int> setInitiative)
		{
			if (Status != BattleStatus.InProgress)
			{
				return false;
			}

			var initiative = Creatures.Count + Characters.Count;
			setInitiative(initiative);

			if (CurrentInitiative is null)
			{
				CurrentInitiative = initiative;
				return true;
			}

			return false;
		}

		public void AddLogEntry(string message)
		{
			LogEntries.Add(new BattleLogEntry(Id, message));
		}

		/// <summary>
		/// Удаляет существо из боя (например, когда его HP падает до нуля — см.
		/// BattleParticipants.RemoveDeadCreatures). См. RemoveParticipant — та же механика передачи
		/// хода/переиндексации, что и у RemoveCharacter.
		/// </summary>
		public void RemoveCreature(Creature creature)
		{
			if (!Creatures.Contains(creature))
			{
				return;
			}

			RemoveParticipant(ParticipantKind.Creature, creature.Id, () => Creatures.Remove(creature));
		}

		/// <summary>
		/// Удаляет персонажа из боя (например, при смерти в конце death-спирали Dying — см.
		/// BattleCombatService.RollDyingSaveAsync). Сам Character остаётся у игрока — удаляется только
		/// запись участия в этом конкретном бою. См. RemoveParticipant.
		/// </summary>
		public void RemoveCharacter(BattleCharacter character)
		{
			if (!Characters.Contains(character))
			{
				return;
			}

			RemoveParticipant(ParticipantKind.Character, character.CharacterId, () => Characters.Remove(character));
		}

		/// <summary>
		/// Переиндексирует Initiative оставшихся участников в плотную последовательность 1..N (её
		/// требует AdvanceTurn) и сохраняет, чей сейчас ход: если удаляемый участник и был активным —
		/// ход передаётся дальше по прежнему порядку (через AdvanceTurn, включая переход раунда), иначе
		/// активный участник остаётся тем же, просто с новым номером инициативы. CurrentInitiative
		/// становится null, если участников не осталось вовсе.
		/// </summary>
		private void RemoveParticipant(ParticipantKind kind, long refId, Action remove)
		{
			if (CurrentInitiative is null)
			{
				remove();
				return;
			}

			var active = FindParticipantByInitiative(CurrentInitiative.Value);
			if (active is { } a && a.Kind == kind && a.RefId == refId)
			{
				AdvanceTurn();
				active = FindParticipantByInitiative(CurrentInitiative!.Value);
			}

			remove();
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

		/// <summary>
		/// Подключает к бою карту (или отключает, если null). Позиции участников относятся к конкретной
		/// карте, поэтому при смене карты все участники снимаются с поля. Доступно при любом статусе боя.
		/// </summary>
		public void AttachMap(BattleMap? battleMap)
		{
			if (battleMap is not null && battleMap.GameId != GameId)
			{
				throw new InvalidArgumentException(ErrorCode.BattleMapBelongsToAnotherGame, "Выбранная карта принадлежит другой игре.");
			}

			var battleMapId = battleMap?.Id;
			if (battleMapId == BattleMapId)
			{
				return;
			}

			BattleMapId = battleMapId;

			foreach (var creature in Creatures)
			{
				creature.RemoveFromMap();
			}

			foreach (var character in Characters)
			{
				character.RemoveFromMap();
			}
		}

		/// <summary>
		/// Выставляет участника на гекс подключённой карты (или переставляет, если он уже на поле). Это
		/// расстановка мастером, а не ход по правилам движения — поэтому разрешена при любом статусе боя.
		/// На гексе может стоять только один участник, и только на проходимом террейне.
		/// </summary>
		/// <param name="participantId">Id существа (Creature.Id) или персонажа (Character.Id) — как в BattleAttack.</param>
		public void PlaceParticipantOnMap(ParticipantKind kind, long participantId, BattleMap battleMap, int column, int row)
		{
			if (BattleMapId is null || battleMap.Id != BattleMapId)
			{
				throw new InvalidArgumentException(ErrorCode.BattleMapNotAttached, "К бою не подключена эта карта.");
			}

			var hex = battleMap.GetHex(column, row);
			if (!hex.IsPassable)
			{
				throw new InvalidArgumentException(ErrorCode.BattleMapHexNotPassable, "На непроходимый гекс участника выставить нельзя.");
			}

			var occupant = FindParticipantAt(column, row);
			if (occupant is { } o && !(o.Kind == kind && o.Id == participantId))
			{
				throw new InvalidArgumentException(ErrorCode.BattleMapHexOccupied, "На этом гексе уже стоит другой участник боя.");
			}

			if (kind == ParticipantKind.Creature)
			{
				GetCreatureForMap(participantId).PlaceOnMap(column, row);
			}
			else
			{
				GetCharacterForMap(participantId).PlaceOnMap(column, row);
			}
		}

		public void RemoveParticipantFromMap(ParticipantKind kind, long participantId)
		{
			if (kind == ParticipantKind.Creature)
			{
				GetCreatureForMap(participantId).RemoveFromMap();
			}
			else
			{
				GetCharacterForMap(participantId).RemoveFromMap();
			}
		}

		/// <summary>Кто стоит на гексе (column, row) подключённой карты, если кто-то стоит.</summary>
		public (ParticipantKind Kind, long Id)? FindParticipantAt(int column, int row)
		{
			var creature = Creatures.FirstOrDefault(c => c.MapColumn == column && c.MapRow == row);
			if (creature is not null)
			{
				return (ParticipantKind.Creature, creature.Id);
			}

			var character = Characters.FirstOrDefault(c => c.MapColumn == column && c.MapRow == row);
			return character is null ? null : (ParticipantKind.Character, character.CharacterId);
		}

		private Creature GetCreatureForMap(long creatureId)
		{
			var creature = Creatures.FirstOrDefault(c => c.Id == creatureId);
			NotFoundException.ThrowIfNull(creature, ErrorCode.CreatureNotFoundInBattle, nameof(Creature), nameof(Creature.Id), creatureId.ToString());
			return creature;
		}

		private BattleCharacter GetCharacterForMap(long characterId)
		{
			var character = Characters.FirstOrDefault(c => c.CharacterId == characterId);
			NotFoundException.ThrowIfNull(character, ErrorCode.BattleCharacterNotFound, nameof(BattleCharacter), nameof(BattleCharacter.CharacterId), characterId.ToString());
			return character;
		}
	}
}
