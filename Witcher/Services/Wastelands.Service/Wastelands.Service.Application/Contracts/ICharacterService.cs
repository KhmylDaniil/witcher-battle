using Wastelands.Service.Application.Models.Dto;
using Wastelands.Service.Application.Models.Filters;
using Wastelands.Service.Application.Models.Requests;

namespace Wastelands.Service.Application.Contracts
{
	public interface ICharacterService
	{
		Task<CharacterDto> GetCharacterByIdAsync(long id);

		Task<List<CharacterDto>> GetCharactersAsync(CharacterFilter filter);

		/// <summary>Персонажи всех игроков этой игры — доступно только мастеру игры.</summary>
		Task<List<CharacterDto>> GetGameCharactersAsync(long gameId);

		Task<CharacterDto> CreateCharacterAsync(CreateCharacterRequest request);

		Task<CharacterDto> UpdateCharacterAsync(UpdateCharacterRequest request);

		Task AddSkillAsync(AddOrUpdateCharacterSkillRequest request);

		Task UpdateSkillAsync(AddOrUpdateCharacterSkillRequest request);

		Task DeleteCharacterAsync(long id);

		Task DeleteSkillAsync(DeleteCharacterSkillRequest request);

		Task<CharacterDto> AddAbilityAsync(CreateCharacterAbilityRequest request);

		Task<CharacterDto> UpdateAbilityAsync(UpdateCharacterAbilityRequest request);

		Task<CharacterDto> RemoveAbilityAsync(long characterId, long abilityId);

		Task<CharacterDto> AddAbilityConditionAsync(AddCharacterAbilityConditionRequest request);

		Task<CharacterDto> UpdateAbilityConditionAsync(UpdateCharacterAbilityConditionRequest request);

		Task<CharacterDto> RemoveAbilityConditionAsync(long characterId, long abilityId, long conditionId);

		Task<CharacterDto> AddAbilityDefensiveSkillAsync(AddCharacterAbilityDefensiveSkillRequest request);

		Task<CharacterDto> RemoveAbilityDefensiveSkillAsync(long characterId, long abilityId, long defensiveSkillId);
	}
}
