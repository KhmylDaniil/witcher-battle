using Wastelands.Core.Contracts.Enums;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Service.Application.Models;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Services
{
	/// <summary>Итог встречного броска — для MarkHitResolved и для подробного лога боя.</summary>
	internal readonly record struct HitResult(bool Succeeded, long? ResolvedCreaturePartId, int AttackRoll, int AttackTotal, int DefenseRoll, int DefenseTotal);

	/// <summary>
	/// Итог расчёта урона. ArmorBeforeHit/ArmorAfterHit/ArmorAbsorbed заполнены, только если защитник —
	/// существо и попадание пришлось в часть тела (PartName не null); у персонажа брони нет.
	/// </summary>
	internal readonly record struct DamageResult(int FinalDamage, string? PartName, int RawDamage, int ArmorBeforeHit, int ArmorAbsorbed, int ArmorAfterHit);

	/// <summary>
	/// Чистая боевая математика (попадание, урон) — без загрузки данных и без сохранения. Вынесена
	/// из BattleCombatService, чтобы там остался только оркестрация (авторизация/загрузка/лог/сохранение).
	/// </summary>
	internal static class BattleCombatCalculator
	{
		private static readonly Random Random = new();

		public static int RollDie(int sides) => Random.Next(1, sides + 1);

		/// <summary>Урон — сумма обычных (не взрывающихся) d6, поэтому диапазон ограничен: от всех единиц до всех шестёрок.</summary>
		public static void ValidateDamageRoll(int roll, Ability ability)
		{
			if (roll < ability.DamageDiceCount || roll > ability.DamageDiceCount * 6)
			{
				throw new InvalidArgumentException(
					ErrorCode.InvalidDamageRoll,
					$"Бросок урона должен быть от {ability.DamageDiceCount} до {ability.DamageDiceCount * 6} (сумма {ability.DamageDiceCount}к6).");
			}
		}

		/// <summary>
		/// Встречный бросок: (характеристика+навык атаки [+модификатор части тела]) + d10, против
		/// (характеристика+навык защиты) + d10. Часть тела — выбранная атакующим, либо (если не выбрана
		/// и защитник — существо) случайная по d10 в диапазон MinToHit..MaxToHit.
		/// </summary>
		public static HitResult ResolveHit(
			ParticipantCombatContext attackerContext,
			ParticipantCombatContext defenderContext,
			Ability ability,
			BattleAttack attack)
		{
			long? resolvedPartId = null;
			var hitPenalty = 0;

			if (attack.DefenderKind == ParticipantKind.Creature)
			{
				var parts = defenderContext.Template!.Parts;
				if (attack.TargetedCreaturePartId is { } chosenPartId)
				{
					resolvedPartId = chosenPartId;
					hitPenalty = parts.First(p => p.Id == chosenPartId).HitPenalty;
				}
				else
				{
					var hitRoll = RollDie(10);
					var part = parts.FirstOrDefault(p => hitRoll >= p.MinToHit && hitRoll <= p.MaxToHit) ?? parts.First();
					resolvedPartId = part.Id;
				}
			}

			var attackRollUsed = attack.AttackRoll ?? RollDie(10);
			var attackTotal = attackerContext.GetSkillValue(ability.AttackSkill) + hitPenalty + attackRollUsed;

			var defenseRollUsed = attack.DefenseRoll ?? RollDie(10);
			var defenseTotal = defenderContext.GetSkillValue(attack.DefensiveSkill!.Value) + defenseRollUsed;

			var succeeded = attackTotal > defenseTotal;
			return new HitResult(succeeded, succeeded ? resolvedPartId : null, attackRollUsed, attackTotal, defenseRollUsed, defenseTotal);
		}

		/// <summary>
		/// Бросок урона способности + модификатор, для существа — умноженный на модификатор части тела
		/// и модификатор типа урона (Vulnerability×2/Resistance÷2/Immunity×0), минус эффективная броня
		/// части (шаблонная броня за вычетом уже накопленного в этом бою износа). Для персонажа — без
		/// частей тела и без брони (см. ключевые решения плана боя).
		/// </summary>
		public static DamageResult CalculateDamage(
			ParticipantCombatContext attackerContext,
			ParticipantCombatContext? defenderContext,
			ParticipantKind defenderKind,
			Ability ability,
			BattleAttack attack)
		{
			var damageRoll = attack.DamageRoll ?? Enumerable.Range(0, ability.DamageDiceCount).Sum(_ => RollDie(6));
			double raw = damageRoll + ability.DamageModifier;

			if (defenderKind != ParticipantKind.Creature)
			{
				var dmg = Math.Max(0, (int)Math.Round(raw));
				return new DamageResult(dmg, null, dmg, 0, 0, 0);
			}

			var part = defenderContext!.Template!.Parts.First(p => p.Id == attack.ResolvedCreaturePartId);
			raw *= part.DamageModifier;

			if (defenderContext.Template.DamageTypeModifiers.TryGetValue(ability.DamageType, out var modifier))
			{
				raw = modifier switch
				{
					DamageTypeModifier.Vulnerability => raw * 2,
					DamageTypeModifier.Resistance => raw / 2,
					DamageTypeModifier.Immunity => 0,
					_ => raw,
				};
			}

			var rawDamage = Math.Max(0, (int)Math.Round(raw));
			var armorReduction = defenderContext.Creature!.GetArmorReduction(part.Id);
			var armorBeforeHit = Math.Max(0, part.Armor - armorReduction);
			var armorAfterHit = Math.Max(0, part.Armor - (armorReduction + 1));
			var armorAbsorbed = Math.Min(rawDamage, armorBeforeHit);
			var finalDamage = Math.Max(0, rawDamage - armorBeforeHit);

			return new DamageResult(finalDamage, part.Name, rawDamage, armorBeforeHit, armorAbsorbed, armorAfterHit);
		}
	}
}
