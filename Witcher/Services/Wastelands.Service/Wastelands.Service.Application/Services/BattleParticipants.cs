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

		/// <summary>Применяет итог урона к защитнику (существо — ещё и износ брони) и накладывает состояния, прошедшие проверку.</summary>
		public static void ApplyDamage(
			Battle battle,
			BattleAttack attack,
			ParticipantKind defenderKind,
			long defenderId,
			DamageResult damage,
			IReadOnlyList<Condition> appliedConditions)
		{
			if (defenderKind == ParticipantKind.Creature)
			{
				var creature = GetCreature(battle, defenderId);
				creature.ApplyDamage(damage.FinalDamage);
				if (damage.PartName is not null)
				{
					creature.WearArmor(attack.ResolvedCreaturePartId!.Value);
				}

				foreach (var condition in appliedConditions)
				{
					creature.AddCondition(condition);
				}
			}
			else
			{
				var character = GetBattleCharacter(battle, defenderId);
				character.ApplyDamage(damage.FinalDamage);

				foreach (var condition in appliedConditions)
				{
					character.AddCondition(condition);
				}
			}
		}
	}
}
