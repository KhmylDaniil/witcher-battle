using Wastelands.Service.Application.Models.Dto;
using Wastelands.Service.Application.Models.Requests;

namespace Wastelands.Service.Application.Contracts
{
	/// <summary>
	/// Процесс атаки внутри уже идущего боя: выбор способности/цели, подтверждения атакующего и
	/// защитника, расчёт попадания и урона, продолжение той же способности (multiattack) или
	/// завершение хода. Отделено от IBattleService (создание/ростер/инициатива), т.к. это другой
	/// смысловой блок с другой моделью авторизации (по контроллеру текущего участника, а не "я мастер").
	/// </summary>
	public interface IBattleCombatService
	{
		Task<BattleDto> StartAttackAsync(StartAttackRequest request);

		Task<BattleDto> SetAttackerChoicesAsync(SetAttackerChoicesRequest request);

		Task<BattleDto> ConfirmAttackerAsync(long battleId);

		Task<BattleDto> SetDefenderChoiceAsync(SetDefenderChoiceRequest request);

		Task<BattleDto> ConfirmDefenderAsync(long battleId);

		Task<BattleDto> SetDamageRollAsync(SetDamageRollRequest request);

		Task<BattleDto> ContinueDamageAsync(long battleId);

		/// <summary>Ручной ввод д10 для stun save защитника, когда этой атакой прошла попытка наложить Оглушение.</summary>
		Task<BattleDto> SetStunSaveRollAsync(SetStunSaveRollRequest request);

		/// <summary>Разрешает stun save защитника — определяет, наложено ли Оглушение этой атакой.</summary>
		Task<BattleDto> ResolveStunSaveAsync(long battleId);

		/// <summary>Stun save в свой ход для уже оглушённого участника — единственное доступное ему действие, всегда заканчивает ход.</summary>
		Task<BattleDto> RollOwnStunSaveAsync(RollOwnStunSaveRequest request);

		Task<BattleDto> NextSwingAsync(NextSwingRequest request);

		Task<BattleDto> EndActivationAsync(long battleId);

		Task<BattleDto> SkipTurnAsync(long battleId);
	}
}
