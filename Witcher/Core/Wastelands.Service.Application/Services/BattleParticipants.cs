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

		/// <summary>
		/// Убирает из боя существ, чей CurrentHP упал до нуля — персонажей это не касается, они не
		/// удаляются автоматически. Существо, сейчас участвующее в незавершённой атаке (атакующий или
		/// защитник), не трогаем до её конца — иначе EnrichAttackAsync/GetContextAsync сломаются на
		/// уже не существующем участнике; такое существо будет убрано следующим же сохранением после
		/// того, как атака закончится или сменит цель (EndActivationAsync/NextSwingAsync).
		/// </summary>
		public static void RemoveDeadCreatures(Battle battle)
		{
			var protectedIds = new HashSet<long>();
			if (battle.Attack is { } attack)
			{
				if (attack.AttackerKind == ParticipantKind.Creature)
				{
					protectedIds.Add(attack.AttackerId);
				}

				if (attack.DefenderKind == ParticipantKind.Creature)
				{
					protectedIds.Add(attack.DefenderId);
				}
			}

			foreach (var creature in battle.Creatures.Where(c => c.CurrentHP <= 0 && !protectedIds.Contains(c.Id)).ToList())
			{
				battle.AddLogEntry($"{creature.Name} погибает и выбывает из боя.");
				battle.RemoveCreature(creature);
			}
		}

		/// <summary>Суммарный штраф к броску атаки/защиты от Ошеломления (-2) и Ослепления (-3) — складываются, если есть оба.</summary>
		public static int GetAttackDefenseModifier(Battle battle, ParticipantKind kind, long participantId)
		{
			var modifier = 0;
			if (HasCondition(battle, kind, participantId, Condition.Staggered))
			{
				modifier -= 2;
			}

			if (HasCondition(battle, kind, participantId, Condition.Blinded))
			{
				modifier -= 3;
			}

			return modifier;
		}

		/// <summary>
		/// Штраф к конкретному навыку от состояний (сейчас — только Ослепление: -5 к Awareness). На
		/// данный момент Awareness нигде в бросках не используется, поэтому этот модификатор пока ни на
		/// что не влияет — задел на будущее (проверки восприятия, определение локации попадания и т.п.).
		/// </summary>
		public static int GetConditionSkillModifier(Battle battle, ParticipantKind kind, long participantId, Skill skill)
		{
			if (skill == Skill.Awareness && HasCondition(battle, kind, participantId, Condition.Blinded))
			{
				return -5;
			}

			return 0;
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

			CheckCharacterDying(battle, defenderKind, defenderId);
		}

		/// <summary>
		/// Периодический урон вне рамок атаки (Bleed/Poison/Fire/Sufflocation в начале хода — см.
		/// BattleTurnProcessor) — та же логика снятия Оглушения при получении урона, что и в ApplyDamage,
		/// но без брони/состояний атаки, которых у периодического урона просто нет.
		/// </summary>
		public static void ApplyPeriodicDamage(Battle battle, ParticipantKind kind, long participantId, int amount)
		{
			if (amount >= 1 && HasCondition(battle, kind, participantId, Condition.Stun))
			{
				RemoveCondition(battle, kind, participantId, Condition.Stun);
			}

			if (kind == ParticipantKind.Creature)
			{
				GetCreature(battle, participantId).ApplyDamage(amount);
			}
			else
			{
				GetBattleCharacter(battle, participantId).ApplyDamage(amount);
			}

			CheckCharacterDying(battle, kind, participantId);
		}

		/// <summary>Персонаж (не существо — те при 0 HP удаляются, см. RemoveDeadCreatures) на 0 HP помечается Dying, пока не помечен.</summary>
		private static void CheckCharacterDying(Battle battle, ParticipantKind kind, long participantId)
		{
			if (kind != ParticipantKind.Character)
			{
				return;
			}

			var battleCharacter = GetBattleCharacter(battle, participantId);
			if (battleCharacter.CurrentHP <= 0 && !battleCharacter.AppliedConditions.Contains(Condition.Dying))
			{
				battleCharacter.AddCondition(Condition.Dying);
				battle.AddLogEntry($"{battleCharacter.Character.Name} при смерти.");
			}
		}
	}
}
