using Wastelands.Service.Application.Models;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Contracts
{
	/// <summary>
	/// Загружает всё, что нужно для боевых расчётов про участника боя (способности, статы/навыки,
	/// у существ — ещё и шаблон целиком). Creature/BattleCharacter намеренно не хранят это сами —
	/// см. комментарий в Creature.cs — поэтому здесь всегда читаем через CreatureTemplateId/CharacterId.
	/// </summary>
	public interface IBattleCombatContextProvider
	{
		Task<ParticipantCombatContext> GetContextAsync(Battle battle, ParticipantKind kind, long participantId);
	}
}
