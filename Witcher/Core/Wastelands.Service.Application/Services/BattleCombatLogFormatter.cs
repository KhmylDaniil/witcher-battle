using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Services
{
	/// <summary>
	/// Текст записей лога боя для промаха/попадания — вынесен из BattleHitResolver/BattleCombatService,
	/// чтобы оба места собирали строку одинаково (броски атаки/защиты, часть тела, броня).
	/// </summary>
	internal static class BattleCombatLogFormatter
	{
		public static string FormatMiss(string attackerName, string abilityName, string defenderName, HitResult hit) =>
			$"{attackerName} ({abilityName}) атакует {defenderName}: {FormatRolls(hit.AttackRoll, hit.AttackTotal, hit.DefenseRoll, hit.DefenseTotal)} — промах.";

		public static string FormatHit(
			string attackerName,
			string abilityName,
			string defenderName,
			BattleAttack attack,
			DamageResult damage,
			IReadOnlyList<Condition> appliedConditions)
		{
			var rolls = FormatRolls(attack.AttackRoll!.Value, attack.AttackTotal, attack.DefenseRoll!.Value, attack.DefenseTotal);
			var conditionsSuffix = appliedConditions.Count > 0
				? $" Наложены состояния: {string.Join(", ", appliedConditions)}."
				: "";

			return damage.PartName is null
				? $"{attackerName} ({abilityName}) атакует {defenderName}: {rolls} — попадание, урон {damage.FinalDamage}.{conditionsSuffix}"
				: $"{attackerName} ({abilityName}) атакует {defenderName} ({damage.PartName}): {rolls} — попадание. "
					+ $"Урон до брони {damage.RawDamage}, броня поглотила {damage.ArmorAbsorbed} (было {damage.ArmorBeforeHit}, после удара {damage.ArmorAfterHit}), "
					+ $"итоговый урон {damage.FinalDamage}.{conditionsSuffix}";
		}

		private static string FormatRolls(int attackRoll, int attackTotal, int defenseRoll, int defenseTotal) =>
			$"бросок атаки {attackRoll} (итого {attackTotal}) против защиты {defenseRoll} (итого {defenseTotal})";
	}
}
