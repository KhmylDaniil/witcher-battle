using Wastelands.Service.Domain.Enums;
using Wastelands.Service.Domain.Models.Dto;
using Wastelands.Service.Domain.Models.Filters;
using Wastelands.Service.Domain.Models.Requests;

namespace Wastelands.Service.Domain.Contracts
{
	public interface ICreatureTemplateService
	{
		Task<CreatureTemplateDto> GetCreatureTemplateByIdAsync(long id);

		Task<List<CreatureTemplateDto>> GetCreatureTemplatesAsync(CreatureTemplateFilter filter);

		Task<CreatureTemplateDto> CreateCreatureTemplateAsync(CreateCreatureTemplateRequest request);

		Task<CreatureTemplateDto> UpdateCreatureTemplateAsync(UpdateCreatureTemplateRequest request);

		Task<CreatureTemplateDto> UpdatePartArmorAsync(long creatureTemplateId, long partId, int armor);

		Task DeleteCreatureTemplateAsync(long id);

		Task AddSkillAsync(AddOrUpdateCreatureTemplateSkillRequest request);

		Task UpdateSkillAsync(AddOrUpdateCreatureTemplateSkillRequest request);

		Task DeleteSkillAsync(DeleteCreatureTemplateSkillRequest request);

		Task SetDamageTypeModifierAsync(SetDamageTypeModifierRequest request);

		Task RemoveDamageTypeModifierAsync(long creatureTemplateId, DamageType damageType);

		Task<CreatureTemplateDto> AddAbilityAsync(CreateAbilityRequest request);

		Task<CreatureTemplateDto> UpdateAbilityAsync(UpdateAbilityRequest request);

		Task<CreatureTemplateDto> RemoveAbilityAsync(long creatureTemplateId, long abilityId);

		Task<CreatureTemplateDto> AddAbilityConditionAsync(AddAbilityConditionRequest request);

		Task<CreatureTemplateDto> UpdateAbilityConditionAsync(UpdateAbilityConditionRequest request);

		Task<CreatureTemplateDto> RemoveAbilityConditionAsync(long creatureTemplateId, long abilityId, long conditionId);

		Task<CreatureTemplateDto> AddAbilityDefensiveSkillAsync(AddAbilityDefensiveSkillRequest request);

		Task<CreatureTemplateDto> RemoveAbilityDefensiveSkillAsync(long creatureTemplateId, long abilityId, long defensiveSkillId);
	}
}
