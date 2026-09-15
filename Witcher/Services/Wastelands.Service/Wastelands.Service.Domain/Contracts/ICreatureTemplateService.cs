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
	}
}
