using Microsoft.AspNetCore.Mvc;
using Wastelands.Service.Domain.Contracts;
using Wastelands.Service.Domain.Models.Dto;
using Wastelands.Service.Domain.Models.Filters;
using Wastelands.Service.Domain.Models.Requests;

namespace Wastelands.Service.MVC.Controllers.Api
{
	/// <summary>Создание и редактирование шаблонов существ — доступно только мастеру игры (см. CreatureTemplateService/CreatureTemplateRepository).</summary>
	[Route("api/creature-templates")]
	public class CreatureTemplatesApiController : ApiControllerBase
	{
		private readonly ICreatureTemplateService _creatureTemplateService;

		public CreatureTemplatesApiController(ICreatureTemplateService creatureTemplateService)
		{
			_creatureTemplateService = creatureTemplateService;
		}

		[HttpGet]
		public async Task<List<CreatureTemplateDto>> Index([FromQuery] CreatureTemplateFilter filter)
			=> await _creatureTemplateService.GetCreatureTemplatesAsync(filter);

		[HttpGet("{id:long}")]
		public async Task<CreatureTemplateDto> Get(long id)
			=> await _creatureTemplateService.GetCreatureTemplateByIdAsync(id);

		[HttpPost]
		public async Task<CreatureTemplateDto> Create(CreateCreatureTemplateRequest request)
			=> await _creatureTemplateService.CreateCreatureTemplateAsync(request);

		[HttpPut("{id:long}")]
		public async Task<CreatureTemplateDto> Update(long id, UpdateCreatureTemplateRequest request)
		{
			request.Id = id;
			return await _creatureTemplateService.UpdateCreatureTemplateAsync(request);
		}

		[HttpDelete("{id:long}")]
		public async Task<IActionResult> Delete(long id)
		{
			await _creatureTemplateService.DeleteCreatureTemplateAsync(id);
			return NoContent();
		}
	}
}
