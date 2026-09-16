using Microsoft.AspNetCore.Mvc;
using Wastelands.Service.Application.Contracts;
using Wastelands.Service.Domain.Enums;
using Wastelands.Service.Application.Models.Dto;
using Wastelands.Service.Application.Models.Filters;
using Wastelands.Service.Application.Models.Requests;

namespace Wastelands.Service.MVC.Controllers.Api
{
	/// <summary>
	/// Просмотр доступен мастеру игры всегда и игрокам — только после начала боя, в котором участвует их
	/// персонаж (см. BattleRepository.GetQuery()). Мутирующие операции — только мастеру игры (BattleService).
	/// </summary>
	[Route("api/games/{gameId:long}/battles")]
	public class BattlesApiController : ApiControllerBase
	{
		private readonly IBattleService _battleService;

		public BattlesApiController(IBattleService battleService)
		{
			_battleService = battleService;
		}

		[HttpGet]
		public async Task<List<BattleDto>> Index(long gameId, [FromQuery] BattleFilter filter)
		{
			filter.GameId = gameId;
			return await _battleService.GetBattlesAsync(filter);
		}

		[HttpGet("{id:long}")]
		public async Task<BattleDto> Get(long gameId, long id)
			=> await _battleService.GetBattleByIdAsync(id);

		[HttpPost]
		public async Task<BattleDto> Create(long gameId, CreateBattleRequest request)
		{
			request.GameId = gameId;
			return await _battleService.CreateBattleAsync(request);
		}

		[HttpDelete("{id:long}")]
		public async Task<IActionResult> Delete(long gameId, long id)
		{
			await _battleService.DeleteBattleAsync(id);
			return NoContent();
		}

		[HttpPost("{id:long}/creatures")]
		public async Task<BattleDto> AddCreature(long gameId, long id, AddCreatureToBattleRequest request)
		{
			request.BattleId = id;
			return await _battleService.AddCreatureAsync(request);
		}

		[HttpPut("{id:long}/creatures/{creatureId:long}")]
		public async Task<BattleDto> UpdateCreature(long gameId, long id, long creatureId, UpdateBattleCreatureRequest request)
		{
			request.BattleId = id;
			request.CreatureId = creatureId;
			return await _battleService.UpdateCreatureAsync(request);
		}

		[HttpDelete("{id:long}/creatures/{creatureId:long}")]
		public async Task<BattleDto> RemoveCreature(long gameId, long id, long creatureId)
			=> await _battleService.RemoveCreatureAsync(id, creatureId);

		[HttpPost("{id:long}/creatures/{creatureId:long}/conditions")]
		public async Task<BattleDto> AddCreatureCondition(long gameId, long id, long creatureId, [FromBody] ConditionPayload payload)
			=> await _battleService.AddCreatureConditionAsync(id, creatureId, payload.Condition);

		[HttpDelete("{id:long}/creatures/{creatureId:long}/conditions/{condition}")]
		public async Task<BattleDto> RemoveCreatureCondition(long gameId, long id, long creatureId, Condition condition)
			=> await _battleService.RemoveCreatureConditionAsync(id, creatureId, condition);

		[HttpPost("{id:long}/characters")]
		public async Task<BattleDto> AddCharacter(long gameId, long id, AddCharacterToBattleRequest request)
		{
			request.BattleId = id;
			return await _battleService.AddCharacterAsync(request);
		}

		[HttpDelete("{id:long}/characters/{characterId:long}")]
		public async Task<BattleDto> RemoveCharacter(long gameId, long id, long characterId)
			=> await _battleService.RemoveCharacterAsync(id, characterId);

		[HttpPost("{id:long}/characters/{characterId:long}/conditions")]
		public async Task<BattleDto> AddCharacterCondition(long gameId, long id, long characterId, [FromBody] ConditionPayload payload)
			=> await _battleService.AddCharacterConditionAsync(id, characterId, payload.Condition);

		[HttpDelete("{id:long}/characters/{characterId:long}/conditions/{condition}")]
		public async Task<BattleDto> RemoveCharacterCondition(long gameId, long id, long characterId, Condition condition)
			=> await _battleService.RemoveCharacterConditionAsync(id, characterId, condition);

		[HttpPost("{id:long}/start")]
		public async Task<BattleDto> Start(long gameId, long id)
			=> await _battleService.StartBattleAsync(id);
	}

	public sealed class ConditionPayload
	{
		public Condition Condition { get; set; }
	}
}
