using Wastelands.Service.Domain.Models.Dto;
using Wastelands.Service.Domain.Models.Filters;
using Wastelands.Service.Domain.Models.Requests;

namespace Wastelands.Service.Domain.Contracts
{
	public interface IBodyTemplateService
	{
		Task<BodyTemplateDto> GetBodyTemplateByIdAsync(long id);

		Task<List<BodyTemplateDto>> GetBodyTemplatesAsync(BodyTemplateFilter filter);

		Task<BodyTemplateDto> CreateBodyTemplateAsync(CreateBodyTemplateRequest request);

		Task<BodyTemplateDto> UpdateBodyTemplateAsync(UpdateBodyTemplateRequest request);

		Task DeleteBodyTemplateAsync(long id);
	}
}
