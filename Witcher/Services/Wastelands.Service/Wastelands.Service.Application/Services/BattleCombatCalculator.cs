using Wastelands.Service.Application.Models;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Services
{
	/// <summary>
	/// Чистая боевая математика (попадание, урон) — без загрузки данных и без сохранения. Вынесена
	/// из BattleCombatService, чтобы там остался только оркестрация (авторизация/загрузка/лог/сохранение).
	/// </summary>
	internal static class BattleCombatCalculator
	{
		private static readonly Random Random = new();

		public static int RollDie(int sides) => Random.Next(1, sides + 1);

		/// <summary>
		/// Встречный бросок: (характеристика+навык атаки [+модификатор части тела]) + d10, против
		/// (характеристика+навык защиты) + d10. Часть тела — выбранная атакующим, либо (если не выбрана
		/// и защитник — существо) случайная по d10 в диапазон MinToHit..MaxToHit.
		/// </summary>
		public static (bool Succeeded, long? ResolvedCreaturePartId) ResolveHit(
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

			var attackTotal = attackerContext.GetSkillValue(ability.AttackSkill) + hitPenalty + (attack.AttackRoll ?? RollDie(10));
			var defenseTotal = defenderContext.GetSkillValue(attack.DefensiveSkill!.Value) + (attack.DefenseRoll ?? RollDie(10));

			var succeeded = attackTotal > defenseTotal;
			return (succeeded, succeeded ? resolvedPartId : null);
		}

		/// <summary>
		/// Бросок урона способности + модификатор, для существа — умноженный на модификатор части тела
		/// и модификатор типа урона (Vulnerability×2/Resistance÷2/Immunity×0), минус броня части тела.
		/// Для персонажа — без частей тела и без брони (см. ключевые решения плана боя).
		/// </summary>
		public static (int FinalDamage, string? PartName) CalculateDamage(
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
				return (Math.Max(0, (int)Math.Round(raw)), null);
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

			return (Math.Max(0, (int)Math.Round(raw) - part.Armor), part.Name);
		}
	}
}
