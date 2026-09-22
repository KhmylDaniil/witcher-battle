using Microsoft.AspNetCore.Mvc;
using Wastelands.Service.Application.Contracts;
using Wastelands.Service.Domain.Enums;
using Wastelands.Service.Application.Models.Dto;
using Wastelands.Service.Application.Models.Filters;
using Wastelands.Service.Application.Models.Requests;

namespace Wastelands.API.Controllers.Api
{
	/// <summary>
	/// Просмотр доступен мастеру игры всегда и игрокам — только после начала боя, в котором участвует их
	/// персонаж (см. BattleRepository.GetQuery()). Мутирующие операции — только мастеру игры (BattleService).
	/// </summary>
	[Route("api/games/{gameId:long}/battles")]
	public class BattlesApiController : ApiControllerBase
	{
		private readonly IBattleService _battleService;
		private readonly IBattleCombatService _battleCombatService;
		private readonly IBattleParticipantSheetService _sheetService;

		public BattlesApiController(IBattleService battleService, IBattleCombatService battleCombatService, IBattleParticipantSheetService sheetService)
		{
			_battleService = battleService;
			_battleCombatService = battleCombatService;
			_sheetService = sheetService;
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

		[HttpPost("{id:long}/attacks")]
		public async Task<BattleDto> StartAttack(long gameId, long id, StartAttackRequest request)
		{
			request.BattleId = id;
			return await _battleCombatService.StartAttackAsync(request);
		}

		[HttpPost("{id:long}/attacks/current/attacker-choices")]
		public async Task<BattleDto> SetAttackerChoices(long gameId, long id, SetAttackerChoicesRequest request)
		{
			request.BattleId = id;
			return await _battleCombatService.SetAttackerChoicesAsync(request);
		}

		[HttpPost("{id:long}/attacks/current/attacker-confirm")]
		public async Task<BattleDto> ConfirmAttacker(long gameId, long id)
			=> await _battleCombatService.ConfirmAttackerAsync(id);

		[HttpPost("{id:long}/attacks/current/defender-choice")]
		public async Task<BattleDto> SetDefenderChoice(long gameId, long id, SetDefenderChoiceRequest request)
		{
			request.BattleId = id;
			return await _battleCombatService.SetDefenderChoiceAsync(request);
		}

		[HttpPost("{id:long}/attacks/current/defender-confirm")]
		public async Task<BattleDto> ConfirmDefender(long gameId, long id)
			=> await _battleCombatService.ConfirmDefenderAsync(id);

		[HttpPost("{id:long}/attacks/current/damage-roll")]
		public async Task<BattleDto> SetDamageRoll(long gameId, long id, SetDamageRollRequest request)
		{
			request.BattleId = id;
			return await _battleCombatService.SetDamageRollAsync(request);
		}

		[HttpPost("{id:long}/attacks/current/continue")]
		public async Task<BattleDto> ContinueDamage(long gameId, long id)
			=> await _battleCombatService.ContinueDamageAsync(id);

		[HttpPost("{id:long}/attacks/current/stun-save-roll")]
		public async Task<BattleDto> SetStunSaveRoll(long gameId, long id, SetStunSaveRollRequest request)
		{
			request.BattleId = id;
			return await _battleCombatService.SetStunSaveRollAsync(request);
		}

		[HttpPost("{id:long}/attacks/current/stun-save-resolve")]
		public async Task<BattleDto> ResolveStunSave(long gameId, long id)
			=> await _battleCombatService.ResolveStunSaveAsync(id);

		[HttpPost("{id:long}/stun-save")]
		public async Task<BattleDto> RollOwnStunSave(long gameId, long id, RollOwnStunSaveRequest request)
		{
			request.BattleId = id;
			return await _battleCombatService.RollOwnStunSaveAsync(request);
		}

		[HttpPost("{id:long}/attacks/current/next-swing")]
		public async Task<BattleDto> NextSwing(long gameId, long id, NextSwingRequest request)
		{
			request.BattleId = id;
			return await _battleCombatService.NextSwingAsync(request);
		}

		[HttpPost("{id:long}/attacks/current/end")]
		public async Task<BattleDto> EndActivation(long gameId, long id)
			=> await _battleCombatService.EndActivationAsync(id);

		[HttpPost("{id:long}/skip-turn")]
		public async Task<BattleDto> SkipTurn(long gameId, long id)
			=> await _battleCombatService.SkipTurnAsync(id);

		[HttpPost("{id:long}/remove-condition")]
		public async Task<BattleDto> AttemptRemoveCondition(long gameId, long id, AttemptRemoveConditionRequest request)
		{
			request.BattleId = id;
			return await _battleCombatService.AttemptRemoveConditionAsync(request);
		}

		[HttpPost("{id:long}/clear-condition")]
		public async Task<BattleDto> ClearCondition(long gameId, long id, ClearConditionRequest request)
		{
			request.BattleId = id;
			return await _battleCombatService.ClearConditionAsync(request);
		}

		[HttpGet("{id:long}/creatures/{creatureId:long}/sheet")]
		public async Task<CreatureTemplateDto> GetCreatureSheet(long gameId, long id, long creatureId)
			=> await _sheetService.GetCreatureSheetAsync(id, creatureId);

		[HttpGet("{id:long}/characters/{characterId:long}/sheet")]
		public async Task<CharacterDto> GetCharacterSheet(long gameId, long id, long characterId)
			=> await _sheetService.GetCharacterSheetAsync(id, characterId);
	}

	public sealed class ConditionPayload
	{
		public Condition Condition { get; set; }
	}
}
