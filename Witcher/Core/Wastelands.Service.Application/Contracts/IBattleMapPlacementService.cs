using Wastelands.Service.Application.Models.Dto;
using Wastelands.Service.Application.Models.Requests;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Contracts
{
	/// <summary>
	/// Карта в бою: подключение карты игры к бою и расстановка участников по гексам. Только мастер игры.
	/// Движения по правилам (стоимость шага, очередь хода) здесь нет — это расстановка.
	/// </summary>
	public interface IBattleMapPlacementService
	{
		Task<BattleMapViewDto> GetMapViewAsync(long battleId);

		Task<BattleMapViewDto> AttachMapAsync(AttachBattleMapRequest request);

		Task<BattleMapViewDto> PlaceParticipantAsync(PlaceParticipantOnMapRequest request);

		Task<BattleMapViewDto> RemoveParticipantAsync(long battleId, ParticipantKind kind, long participantId);
	}
}
