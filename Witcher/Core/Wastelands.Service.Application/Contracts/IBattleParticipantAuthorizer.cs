using Wastelands.Core.Contracts.Enums;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Contracts
{
	/// <summary>
	/// "Кто контролирует участника боя" — существом всегда распоряжается мастер игры (у существ нет
	/// владельца-игрока), персонажем — игрок, которому он принадлежит. Общая проверка для всех
	/// действий боевого цикла (StartAttack, подтверждения, skip-turn и т.д.).
	/// </summary>
	public interface IBattleParticipantAuthorizer
	{
		/// <summary>Бросает InvalidArgumentException(errorCode), если текущий пользователь не контролирует участника.</summary>
		Task EnsureControllerAsync(Battle battle, ParticipantKind kind, long participantId, ErrorCode errorCode);
	}
}
