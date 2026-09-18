using Wastelands.Core.Contracts.Models;
using Wastelands.Service.Application.Models.Dto;
using Wastelands.Service.Application.Models.Filters;
using Wastelands.Service.Application.Models.Requests;

namespace Wastelands.Service.Application.Contracts
{
	public interface IBodyTemplateService
	{
		Task<BodyTemplateDto> GetBodyTemplateByIdAsync(long id);

		Task<PagedResultDto<BodyTemplateDto>> GetBodyTemplatesAsync(BodyTemplateFilter filter, PagedRequest paging);

		Task<BodyTemplateDto> CreateBodyTemplateAsync(CreateBodyTemplateRequest request);

		Task<BodyTemplateDto> UpdateBodyTemplateAsync(UpdateBodyTemplateRequest request);

		Task DeleteBodyTemplateAsync(long id);

		Task<BodyTemplateDto> AddPartAsync(CreateBodyTemplatePartRequest request);

		Task<BodyTemplateDto> UpdatePartAsync(UpdateBodyTemplatePartRequest request);

		Task<BodyTemplateDto> RemovePartAsync(long bodyTemplateId, long partId);
	}
}
