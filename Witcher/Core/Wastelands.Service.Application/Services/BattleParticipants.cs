using Wastelands.Core.Contracts.Enums;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Service.Application.Models;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Services
{
	/// <summary>
	/// Поиск участника уже загруженного Battle по id — переиспользуется BattleService, BattleCombatService
	/// и BattleCombatContextProvider. Чисто на данных Battle, без обращений к репозиториям.
	/// </summary>
	internal static class BattleParticipants
	{
		public static Creature GetCreature(Battle battle, long creatureId)
		{
			var creature = battle.Creatures.FirstOrDefault(x => x.Id == creatureId);

			NotFoundException.ThrowIfNull(
				creature, ErrorCode.CreatureNotFoundInBattle, nameof(Creature), nameof(Creature.Id), creatureId.ToString());

			return creature;
		}

		public static BattleCharacter GetBattleCharacter(Battle battle, long characterId)
		{
			var battleCharacter = battle.Characters.FirstOrDefault(x => x.CharacterId == characterId);

			NotFoundException.ThrowIfNull(
				battleCharacter, ErrorCode.BattleCharacterNotFound, nameof(BattleCharacter), nameof(BattleCharacter.CharacterId), characterId.ToString());

			return battleCharacter;
		}

		public static string GetName(Battle battle, ParticipantKind kind, long participantId)
		{
			return kind == ParticipantKind.Creature
				? GetCreature(battle, participantId).Name
				: GetBattleCharacter(battle, participantId).Character.Name;
		}

		public static void EnsureExists(Battle battle, ParticipantKind kind, long participantId)
		{
			if (kind == ParticipantKind.Creature)
			{
				GetCreature(battle, participantId);
			}
			else
			{
				GetBattleCharacter(battle, participantId);
			}
		}

		/// <summary>Участник, чей сейчас ход — определяется по Battle.CurrentInitiative.</summary>
		public static (ParticipantKind Kind, long Id) GetActive(Battle battle)
		{
			if (battle.CurrentInitiative is null)
			{
				throw new InvalidArgumentException(ErrorCode.BattleNotInProgress, "Бой ещё не начат.");
			}

			var creature = battle.Creatures.FirstOrDefault(c => c.Initiative == battle.CurrentInitiative);
			if (creature is not null)
			{
				return (ParticipantKind.Creature, creature.Id);
			}

			var character = battle.Characters.FirstOrDefault(c => c.Initiative == battle.CurrentInitiative);
			if (character is not null)
			{
				return (ParticipantKind.Character, character.CharacterId);
			}

			throw new InvalidArgumentException(ErrorCode.NotYourTurn, "Не найден активный по инициативе участник боя.");
		}

		public static BattleAttack GetActiveAttack(Battle battle)
		{
			if (battle.Attack is null)
			{
				throw new InvalidArgumentException(ErrorCode.NoActiveAttack, "В бою сейчас нет незавершённой атаки.");
			}

			return battle.Attack;
		}

		public static void EnsureInProgress(Battle battle)
		{
			if (battle.Status != BattleStatus.InProgress)
			{
				throw new InvalidArgumentException(ErrorCode.BattleNotInProgress, "Бой ещё не начат.");
			}
		}

		/// <summary>Целиться можно только в часть тела существа, действительно существующую у его текущего шаблона.</summary>
		public static void EnsureTargetedPartValid(ParticipantCombatContext defenderContext, ParticipantKind defenderKind, long partId)
		{
			if (defenderKind != ParticipantKind.Creature)
			{
				throw new InvalidArgumentException(ErrorCode.CreatureTemplatePartNotFound, "У защищающегося персонажа нет частей тела.");
			}

			if (defenderContext.Template!.Parts.All(p => p.Id != partId))
			{
				throw new InvalidArgumentException(ErrorCode.CreatureTemplatePartNotFound, "Часть тела не найдена у защитника.");
			}
		}

		/// <summary>Целиться в часть тела по HumanBodyPart можно, только если защитник — персонаж (у существ — свои части, см. выше).</summary>
		public static void EnsureTargetedHumanBodyPartValid(ParticipantKind defenderKind)
		{
			if (defenderKind != ParticipantKind.Character)
			{
				throw new InvalidArgumentException(ErrorCode.InvalidTargetedBodyPart, "У защищающегося существа нет фиксированной анатомии персонажа.");
			}
		}

		public static bool HasCondition(Battle battle, ParticipantKind kind, long participantId, Condition condition)
		{
			return kind == ParticipantKind.Creature
				? GetCreature(battle, participantId).AppliedConditions.Contains(condition)
				: GetBattleCharacter(battle, participantId).AppliedConditions.Contains(condition);
		}

		public static void AddCondition(Battle battle, ParticipantKind kind, long participantId, Condition condition)
		{
			if (kind == ParticipantKind.Creature)
			{
				GetCreature(battle, participantId).AddCondition(condition);
			}
			else
			{
				GetBattleCharacter(battle, participantId).AddCondition(condition);
			}
		}

		public static void RemoveCondition(Battle battle, ParticipantKind kind, long participantId, Condition condition)
		{
			if (kind == ParticipantKind.Creature)
			{
				GetCreature(battle, participantId).RemoveCondition(condition);
			}
			else
			{
				GetBattleCharacter(battle, participantId).RemoveCondition(condition);
			}
		}

		public static void ApplyCriticalWound(Battle battle, ParticipantKind kind, long participantId, string slotKey, Condition wound)
		{
			if (kind == ParticipantKind.Creature)
			{
				GetCreature(battle, participantId).ApplyCriticalWound(slotKey, wound);
			}
			else
			{
				GetBattleCharacter(battle, participantId).ApplyCriticalWound(slotKey, wound);
			}
		}

		/// <summary>
		/// Применяет итог урона к защитнику (существо — ещё и износ брони) и накладывает состояния,
		/// прошедшие проверку (Condition.Stun сюда не попадает — она проходит через отдельный stun save,
		/// см. BattleCombatService.ContinueDamageAsync). Получение урона снимает уже наложенное Оглушение.
		/// </summary>
		public static void ApplyDamage(
			Battle battle,
			BattleAttack attack,
			ParticipantKind defenderKind,
			long defenderId,
			DamageResult damage,
			IReadOnlyList<Condition> appliedConditions)
		{
			if (damage.FinalDamage >= 1 && HasCondition(battle, defenderKind, defenderId, Condition.Stun))
			{
				RemoveCondition(battle, defenderKind, defenderId, Condition.Stun);
			}

			if (defenderKind == ParticipantKind.Creature)
			{
				var creature = GetCreature(battle, defenderId);
				creature.ApplyDamage(damage.FinalDamage);
				if (damage.PartName is not null)
				{
					creature.WearArmor(attack.ResolvedCreaturePartId!.Value);
				}
			}
			else
			{
				var character = GetBattleCharacter(battle, defenderId);
				character.ApplyDamage(damage.FinalDamage);
			}

			foreach (var condition in appliedConditions)
			{
				AddCondition(battle, defenderKind, defenderId, condition);
			}
		}
	}
}
