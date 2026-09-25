using Microsoft.AspNetCore.Mvc;
using Wastelands.Service.Application.Contracts;
using Wastelands.Service.Application.Models.Dto;
using Wastelands.Service.Application.Models.Requests;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.API.Controllers.Api
{
	/// <summary>Карта в бою: подключение карты игры к бою и расстановка участников — только мастер игры (см. BattleMapPlacementService).</summary>
	[Route("api/games/{gameId:long}/battles/{battleId:long}/map")]
	public class BattleMapPlacementApiController : ApiControllerBase
	{
		private readonly IBattleMapPlacementService _placementService;

		public BattleMapPlacementApiController(IBattleMapPlacementService placementService)
		{
			_placementService = placementService;
		}

		[HttpGet]
		public async Task<BattleMapViewDto> Get(long gameId, long battleId)
			=> await _placementService.GetMapViewAsync(battleId);

		[HttpPut]
		public async Task<BattleMapViewDto> Attach(long gameId, long battleId, AttachBattleMapRequest request)
		{
			request.BattleId = battleId;
			return await _placementService.AttachMapAsync(request);
		}

		[HttpPut("participants/{kind}/{participantId:long}")]
		public async Task<BattleMapViewDto> Place(long gameId, long battleId, ParticipantKind kind, long participantId, PlaceParticipantOnMapRequest request)
		{
			request.BattleId = battleId;
			request.Kind = kind;
			request.ParticipantId = participantId;
			return await _placementService.PlaceParticipantAsync(request);
		}

		[HttpDelete("participants/{kind}/{participantId:long}")]
		public async Task<BattleMapViewDto> Remove(long gameId, long battleId, ParticipantKind kind, long participantId)
			=> await _placementService.RemoveParticipantAsync(battleId, kind, participantId);
	}
}
