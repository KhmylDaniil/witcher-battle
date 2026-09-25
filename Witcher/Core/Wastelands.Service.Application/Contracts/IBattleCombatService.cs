using Wastelands.Service.Application.Models.Dto;
using Wastelands.Service.Application.Models.Requests;
using Wastelands.Service.Domain.Enums;

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

		/// <summary>Попытка снять состояние (Кровотечение/Отравление) броском навыка — отдельное действие хода, не атака.</summary>
		Task<BattleDto> AttemptRemoveConditionAsync(AttemptRemoveConditionRequest request);

		/// <summary>Снятие состояния действием без броска — всегда успешно и только с себя (Огонь, Падение).</summary>
		Task<BattleDto> ClearConditionAsync(ClearConditionRequest request);

		/// <summary>Проверка на смерть для умирающего персонажа (Condition.Dying) — единственное доступное ему в свой ход действие.</summary>
		Task<BattleDto> RollDyingSaveAsync(RollDyingSaveRequest request);

		/// <summary>Попытка стабилизировать умирающего персонажа броском FirstAid против сложности |его HP|.</summary>
		Task<BattleDto> StabilizeAsync(StabilizeRequest request);

		/// <summary>
		/// Перемещение по подключённой к бою карте в свой ход — тратит очки движения, не является
		/// действием хода (в отличие от атаки/снятия состояния — ход не заканчивает и не открывает окно
		/// дополнительного действия), поэтому можно двигаться несколькими вызовами за один ход, пока
		/// хватает запаса.
		/// </summary>
		Task<BattleDto> MoveAsync(MoveParticipantRequest request);

		/// <summary>
		/// Гексы, на которые активный участник может дойти прямо сейчас, вместе со стоимостью пути до
		/// каждого — для подсветки на карте. Доступно и не в свой ход (просмотр), фактически передвинуться
		/// может только контроллер активного участника в его ход (см. MoveAsync).
		/// </summary>
		Task<List<MovementRangeHexDto>> GetMovementRangeAsync(long battleId, ParticipantKind kind, long participantId);

		/// <summary>
		/// Обновляет запас движения участника до базового значения — основное или дополнительное действие
		/// хода (та же механика TryChargeBonusAction, что и у AttemptRemoveConditionAsync): у персонажа
		/// первый вызов в ходу — обычное действие (окно доп. действия открывается), второй — доп. действие
		/// за стамину и заканчивает ход; у существа любой вызов сразу заканчивает ход.
		/// </summary>
		Task<BattleDto> RefreshMovementAsync(long battleId);
	}
}
