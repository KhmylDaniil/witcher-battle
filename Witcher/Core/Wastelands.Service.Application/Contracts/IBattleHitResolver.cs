using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Contracts
{
	/// <summary>
	/// Разрешение попадания, как только обе стороны подтвердили выбор — и список защитных навыков,
	/// допустимых против конкретной способности (используется и для расчёта, и для валидации выбора
	/// защитника). Математика самого броска — в BattleCombatCalculator; здесь только оркестрация
	/// (подгрузка контекстов через IBattleCombatContextProvider, запись результата, лог при промахе).
	/// </summary>
	public interface IBattleHitResolver
	{
		/// <summary>Ничего не делает, пока не подтвердили обе стороны.</summary>
		Task ResolveIfBothConfirmedAsync(Battle battle, BattleAttack attack);

		Task<List<Skill>> GetAvailableDefensiveSkillsAsync(Battle battle, BattleAttack attack);
	}
}
