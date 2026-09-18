using Wastelands.Service.Application.Models.Dto;
using Wastelands.Service.Application.Models.Requests;

namespace Wastelands.Service.Application.Contracts
{
	/// <summary>
	/// Способности персонажа и их наложенные эффекты/защитные навыки — отдельно от ICharacterService
	/// (лист персонажа/навыки), чтобы держать оба файла читаемыми по размеру.
	/// </summary>
	public interface ICharacterAbilityService
	{
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
