using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Application.Contracts
{
	/// <summary>
	/// Критический провал (fumble) броска атаки/защиты — см. BattleFumbleResolver для правил. Должна
	/// вызываться из BattleCombatService в каждой точке, где выпад достигает SwingResolved (промах,
	/// урон без Оглушения, разрешение обычного/фамбл-stun-save), см. вызовы в ConfirmAttackerAsync/
	/// ConfirmDefenderAsync/ContinueDamageAsync/ResolveStunSaveAsync.
	/// </summary>
	public interface IBattleFumbleResolver
	{
		/// <summary>
		/// Идемпотентно проверяет и применяет фамблы этого выпада — по AttackRoll атакующего и
		/// DefenseRoll защитника, по одному разу для каждой стороны (см. BattleAttack.
		/// AttackerFumbleResolved/DefenderFumbleResolved). Если фамбл требует интерактивной проверки
		/// Оглушения — открывает её (Phase становится AwaitingStunSave) и возвращает true: вызывающий
		/// код должен остановиться, не трогая Phase дальше. false — выпад действительно завершён
		/// (SwingResolved), других фамблов ждать не нужно.
		/// </summary>
		Task<bool> FinalizeSwingAsync(Battle battle, BattleAttack attack);
	}
}
