using Microsoft.AspNetCore.Mvc;
using Wastelands.Core.Contracts.Models;
using Wastelands.Service.Application.Contracts;
using Wastelands.Service.Application.Models.Dto;
using Wastelands.Service.Application.Models.Filters;
using Wastelands.Service.Application.Models.Requests;

namespace Wastelands.API.Controllers.Api
{
	/// <summary>Гексагональные карты боя — создаёт и редактирует только мастер игры (см. BattleMapService/BattleMapRepository).</summary>
	[Route("api/battle-maps")]
	public class BattleMapsApiController : ApiControllerBase
	{
		private readonly IBattleMapService _battleMapService;

		public BattleMapsApiController(IBattleMapService battleMapService)
		{
			_battleMapService = battleMapService;
		}

		[HttpGet]
		public async Task<PagedResultDto<BattleMapSummaryDto>> Index([FromQuery] BattleMapFilter filter, [FromQuery] PagedRequest paging)
			=> await _battleMapService.GetBattleMapsAsync(filter, paging);

		[HttpGet("{id:long}")]
		public async Task<BattleMapDto> Get(long id)
			=> await _battleMapService.GetBattleMapByIdAsync(id);

		[HttpPost]
		public async Task<BattleMapDto> Create(CreateBattleMapRequest request)
			=> await _battleMapService.CreateBattleMapAsync(request);

		[HttpPut("{id:long}")]
		public async Task<BattleMapDto> Update(long id, UpdateBattleMapRequest request)
		{
			request.Id = id;
			return await _battleMapService.UpdateBattleMapAsync(request);
		}

		[HttpPut("{battleMapId:long}/hexes")]
		public async Task<BattleMapDto> PaintHexes(long battleMapId, PaintBattleMapHexesRequest request)
		{
			request.BattleMapId = battleMapId;
			return await _battleMapService.PaintHexesAsync(request);
		}

		[HttpPut("{battleMapId:long}/hexes/{column:int}/{row:int}/marker")]
		public async Task<BattleMapDto> SetMarker(long battleMapId, int column, int row, SetBattleMapMarkerRequest request)
		{
			request.BattleMapId = battleMapId;
			request.Column = column;
			request.Row = row;
			return await _battleMapService.SetMarkerAsync(request);
		}

		[HttpDelete("{battleMapId:long}/hexes/{column:int}/{row:int}/marker")]
		public async Task<BattleMapDto> RemoveMarker(long battleMapId, int column, int row)
			=> await _battleMapService.RemoveMarkerAsync(battleMapId, column, row);

		[HttpDelete("{id:long}")]
		public async Task<IActionResult> Delete(long id)
		{
			await _battleMapService.DeleteBattleMapAsync(id);
			return NoContent();
		}
	}
}
