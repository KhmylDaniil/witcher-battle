using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Application.Services
{
	/// <summary>
	/// Расчёт инициативы при старте боя — чистая логика на уже загруженном Battle, вынесена из
	/// BattleService.StartBattleAsync, чтобы там остался только сценарий (проверки/сохранение).
	/// </summary>
	internal static class BattleInitiativeRoller
	{
		private static readonly Random Random = new();

		/// <summary>
		/// Инициатива = бросок (Ref/Rea + d10). При равенстве броска выигрывает более высокий Ref/Rea,
		/// при дальнейшем равенстве — персонаж (а не существо), при дальнейшем — случайно (tiebreak).
		/// Итоговый порядок всегда однозначен, поэтому в Initiative сохраняется не сырой бросок,
		/// а порядковый номер (1, 2, 3...) — до конца боя он больше не меняется.
		/// </summary>
		public static void RollInitiative(Battle battle)
		{
			var rolled = battle.Creatures
				.Select(c => (
					setInitiative: (Action<int>)(v => c.SetInitiative(v)),
					isCharacter: false,
					stat: c.Ref,
					roll: c.Ref + BattleCombatCalculator.RollDie(10),
					tiebreak: Random.Next()))
				.Concat(battle.Characters.Select(bc => (
					setInitiative: (Action<int>)(v => bc.SetInitiative(v)),
					isCharacter: true,
					stat: bc.Character.Rea,
					roll: bc.Character.Rea + BattleCombatCalculator.RollDie(10),
					tiebreak: Random.Next())))
				.OrderByDescending(x => x.roll)
				.ThenByDescending(x => x.stat)
				.ThenByDescending(x => x.isCharacter)
				.ThenBy(x => x.tiebreak)
				.ToList();

			for (var i = 0; i < rolled.Count; i++)
			{
				rolled[i].setInitiative(i + 1);
			}
		}
	}
}
