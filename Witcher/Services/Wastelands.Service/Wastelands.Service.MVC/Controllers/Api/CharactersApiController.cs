using Microsoft.AspNetCore.Mvc;
using Wastelands.Service.Application.Contracts;
using Wastelands.Service.Domain.Enums;
using Wastelands.Service.Application.Models.Dto;
using Wastelands.Service.Application.Models.Filters;
using Wastelands.Service.Application.Models.Requests;

namespace Wastelands.Service.MVC.Controllers.Api
{
	/// <summary>
	/// Тонкая JSON-обёртка над ICharacterService. Чтение/изменение/удаление персонажа и его навыков
	/// скоупится на текущего пользователя внутри CharacterRepository.GetQuery() — see GameCharacters
	/// ниже для GM-варианта без этого скоупинга.
	/// </summary>
	[Route("api/characters")]
	public class CharactersApiController : ApiControllerBase
	{
		private readonly ICharacterService _characterService;

		public CharactersApiController(ICharacterService characterService)
		{
			_characterService = characterService;
		}

		[HttpGet]
		public async Task<List<CharacterDto>> Index([FromQuery] CharacterFilter filter)
			=> await _characterService.GetCharactersAsync(filter);

		[HttpGet("{id:long}")]
		public async Task<CharacterDto> Get(long id)
			=> await _characterService.GetCharacterByIdAsync(id);

		/// <summary>Персонажи всех игроков этой игры — например, для добавления в бой мастером.</summary>
		[HttpGet("/api/games/{gameId:long}/characters")]
		public async Task<List<CharacterDto>> GameCharacters(long gameId)
			=> await _characterService.GetGameCharactersAsync(gameId);

		[HttpPost]
		public async Task<CharacterDto> Create(CreateCharacterRequest request)
			=> await _characterService.CreateCharacterAsync(request);

		[HttpPut("{id:long}")]
		public async Task<CharacterDto> Update(long id, UpdateCharacterRequest request)
		{
			request.Id = id;
			return await _characterService.UpdateCharacterAsync(request);
		}

		[HttpDelete("{id:long}")]
		public async Task<IActionResult> Delete(long id)
		{
			await _characterService.DeleteCharacterAsync(id);
			return NoContent();
		}

		/// <summary>Создание/изменение навыка (upsert) — AddSkillAsync кидает на дубликат, поэтому сперва проверяем</summary>
		[HttpPut("{characterId:long}/skills")]
		public async Task<IActionResult> UpsertSkill(long characterId, [FromBody] SkillPayload payload)
		{
			var request = new AddOrUpdateCharacterSkillRequest { CharacterId = characterId, Skill = payload.Skill, Value = payload.Value };

			var character = await _characterService.GetCharacterByIdAsync(characterId);
			if (character.Skills.ContainsKey(payload.Skill))
				await _characterService.UpdateSkillAsync(request);
			else
				await _characterService.AddSkillAsync(request);

			return Ok();
		}

		[HttpDelete("{characterId:long}/skills/{skill}")]
		public async Task<IActionResult> DeleteSkill(long characterId, Skill skill)
		{
			await _characterService.DeleteSkillAsync(new DeleteCharacterSkillRequest { Id = characterId, Skill = skill });
			return NoContent();
		}

		[HttpPost("{characterId:long}/abilities")]
		public async Task<CharacterDto> AddAbility(long characterId, CreateCharacterAbilityRequest request)
		{
			request.CharacterId = characterId;
			return await _characterService.AddAbilityAsync(request);
		}

		[HttpPut("{characterId:long}/abilities/{abilityId:long}")]
		public async Task<CharacterDto> UpdateAbility(long characterId, long abilityId, UpdateCharacterAbilityRequest request)
		{
			request.CharacterId = characterId;
			request.AbilityId = abilityId;
			return await _characterService.UpdateAbilityAsync(request);
		}

		[HttpDelete("{characterId:long}/abilities/{abilityId:long}")]
		public async Task<CharacterDto> RemoveAbility(long characterId, long abilityId)
			=> await _characterService.RemoveAbilityAsync(characterId, abilityId);

		[HttpPost("{characterId:long}/abilities/{abilityId:long}/conditions")]
		public async Task<CharacterDto> AddCondition(long characterId, long abilityId, AddCharacterAbilityConditionRequest request)
		{
			request.CharacterId = characterId;
			request.AbilityId = abilityId;
			return await _characterService.AddAbilityConditionAsync(request);
		}

		[HttpPut("{characterId:long}/abilities/{abilityId:long}/conditions/{conditionId:long}")]
		public async Task<CharacterDto> UpdateCondition(long characterId, long abilityId, long conditionId, UpdateCharacterAbilityConditionRequest request)
		{
			request.CharacterId = characterId;
			request.AbilityId = abilityId;
			request.ConditionId = conditionId;
			return await _characterService.UpdateAbilityConditionAsync(request);
		}

		[HttpDelete("{characterId:long}/abilities/{abilityId:long}/conditions/{conditionId:long}")]
		public async Task<CharacterDto> RemoveCondition(long characterId, long abilityId, long conditionId)
			=> await _characterService.RemoveAbilityConditionAsync(characterId, abilityId, conditionId);

		[HttpPost("{characterId:long}/abilities/{abilityId:long}/defensive-skills")]
		public async Task<CharacterDto> AddDefensiveSkill(long characterId, long abilityId, AddCharacterAbilityDefensiveSkillRequest request)
		{
			request.CharacterId = characterId;
			request.AbilityId = abilityId;
			return await _characterService.AddAbilityDefensiveSkillAsync(request);
		}

		[HttpDelete("{characterId:long}/abilities/{abilityId:long}/defensive-skills/{defensiveSkillId:long}")]
		public async Task<CharacterDto> RemoveDefensiveSkill(long characterId, long abilityId, long defensiveSkillId)
			=> await _characterService.RemoveAbilityDefensiveSkillAsync(characterId, abilityId, defensiveSkillId);
	}

	public sealed class SkillPayload
	{
		public Skill Skill { get; set; }
		public int Value { get; set; }
	}
}
