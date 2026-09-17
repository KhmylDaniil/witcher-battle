using Wastelands.Service.Application.Models.Dto;
using Wastelands.Service.Application.Models.Requests;

namespace Wastelands.Service.Application.Contracts
{
	/// <summary>
	/// Способности шаблона существа и их наложенные эффекты/защитные навыки — отдельно от
	/// ICreatureTemplateService (статблок/части тела/навыки/модификаторы урона), чтобы держать оба
	/// файла читаемыми по размеру.
	/// </summary>
	public interface ICreatureTemplateAbilityService
	{
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
