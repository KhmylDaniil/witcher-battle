using Wastelands.Core.Contracts.Models;
using Wastelands.Service.Domain.Enums;
using Wastelands.Service.Application.Models.Dto;
using Wastelands.Service.Application.Models.Filters;
using Wastelands.Service.Application.Models.Requests;

namespace Wastelands.Service.Application.Contracts
{
	/// <summary>Статблок, части тела, навыки, модификаторы урона. Способности — в ICreatureTemplateAbilityService.</summary>
	public interface ICreatureTemplateService
	{
		Task<CreatureTemplateDto> GetCreatureTemplateByIdAsync(long id);

		Task<PagedResultDto<CreatureTemplateDto>> GetCreatureTemplatesAsync(CreatureTemplateFilter filter, PagedRequest paging);

		Task<CreatureTemplateDto> CreateCreatureTemplateAsync(CreateCreatureTemplateRequest request);

		Task<CreatureTemplateDto> UpdateCreatureTemplateAsync(UpdateCreatureTemplateRequest request);

		Task<CreatureTemplateDto> UpdatePartArmorAsync(long creatureTemplateId, long partId, int armor);

		Task DeleteCreatureTemplateAsync(long id);

		Task AddSkillAsync(AddOrUpdateCreatureTemplateSkillRequest request);

		Task UpdateSkillAsync(AddOrUpdateCreatureTemplateSkillRequest request);

		Task DeleteSkillAsync(DeleteCreatureTemplateSkillRequest request);

		Task SetDamageTypeModifierAsync(SetDamageTypeModifierRequest request);

		Task RemoveDamageTypeModifierAsync(long creatureTemplateId, DamageType damageType);
	}
}
