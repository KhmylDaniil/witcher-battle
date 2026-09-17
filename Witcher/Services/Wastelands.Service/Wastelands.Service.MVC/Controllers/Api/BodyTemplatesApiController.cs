using Microsoft.AspNetCore.Mvc;
using Wastelands.Core.Contracts.Models;
using Wastelands.Service.Application.Contracts;
using Wastelands.Service.Application.Models.Dto;
using Wastelands.Service.Application.Models.Filters;
using Wastelands.Service.Application.Models.Requests;

namespace Wastelands.Service.MVC.Controllers.Api
{
	/// <summary>Создание и редактирование шаблонов тела — доступно только мастеру игры (см. BodyTemplateService/BodyTemplateRepository).</summary>
	[Route("api/body-templates")]
	public class BodyTemplatesApiController : ApiControllerBase
	{
		private readonly IBodyTemplateService _bodyTemplateService;

		public BodyTemplatesApiController(IBodyTemplateService bodyTemplateService)
		{
			_bodyTemplateService = bodyTemplateService;
		}

		[HttpGet]
		public async Task<PagedResultDto<BodyTemplateDto>> Index([FromQuery] BodyTemplateFilter filter, [FromQuery] PagedRequest paging)
			=> await _bodyTemplateService.GetBodyTemplatesAsync(filter, paging);

		[HttpGet("{id:long}")]
		public async Task<BodyTemplateDto> Get(long id)
			=> await _bodyTemplateService.GetBodyTemplateByIdAsync(id);

		[HttpPost]
		public async Task<BodyTemplateDto> Create(CreateBodyTemplateRequest request)
			=> await _bodyTemplateService.CreateBodyTemplateAsync(request);

		[HttpPut("{id:long}")]
		public async Task<BodyTemplateDto> Update(long id, UpdateBodyTemplateRequest request)
		{
			request.Id = id;
			return await _bodyTemplateService.UpdateBodyTemplateAsync(request);
		}

		[HttpDelete("{id:long}")]
		public async Task<IActionResult> Delete(long id)
		{
			await _bodyTemplateService.DeleteBodyTemplateAsync(id);
			return NoContent();
		}

		[HttpPost("{bodyTemplateId:long}/parts")]
		public async Task<BodyTemplateDto> AddPart(long bodyTemplateId, CreateBodyTemplatePartRequest request)
		{
			request.BodyTemplateId = bodyTemplateId;
			return await _bodyTemplateService.AddPartAsync(request);
		}

		[HttpPut("{bodyTemplateId:long}/parts/{partId:long}")]
		public async Task<BodyTemplateDto> UpdatePart(long bodyTemplateId, long partId, UpdateBodyTemplatePartRequest request)
		{
			request.BodyTemplateId = bodyTemplateId;
			request.PartId = partId;
			return await _bodyTemplateService.UpdatePartAsync(request);
		}

		[HttpDelete("{bodyTemplateId:long}/parts/{partId:long}")]
		public async Task<BodyTemplateDto> RemovePart(long bodyTemplateId, long partId)
			=> await _bodyTemplateService.RemovePartAsync(bodyTemplateId, partId);
	}
}
