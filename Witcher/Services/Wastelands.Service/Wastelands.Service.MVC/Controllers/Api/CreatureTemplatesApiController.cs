using Microsoft.AspNetCore.Mvc;
using Wastelands.Core.Contracts.Models;
using Wastelands.Service.Application.Contracts;
using Wastelands.Service.Domain.Enums;
using Wastelands.Service.Application.Models.Dto;
using Wastelands.Service.Application.Models.Filters;
using Wastelands.Service.Application.Models.Requests;

namespace Wastelands.Service.MVC.Controllers.Api
{
	/// <summary>Создание и редактирование шаблонов существ — доступно только мастеру игры (см. CreatureTemplateService/CreatureTemplateRepository).</summary>
	[Route("api/creature-templates")]
	public class CreatureTemplatesApiController : ApiControllerBase
	{
		private readonly ICreatureTemplateService _creatureTemplateService;
		private readonly ICreatureTemplateAbilityService _creatureTemplateAbilityService;
		private readonly ICreatureTemplateImageService _creatureTemplateImageService;

		public CreatureTemplatesApiController(
			ICreatureTemplateService creatureTemplateService,
			ICreatureTemplateAbilityService creatureTemplateAbilityService,
			ICreatureTemplateImageService creatureTemplateImageService)
		{
			_creatureTemplateService = creatureTemplateService;
			_creatureTemplateAbilityService = creatureTemplateAbilityService;
			_creatureTemplateImageService = creatureTemplateImageService;
		}

		[HttpGet]
		public async Task<PagedResultDto<CreatureTemplateDto>> Index([FromQuery] CreatureTemplateFilter filter, [FromQuery] PagedRequest paging)
			=> await _creatureTemplateService.GetCreatureTemplatesAsync(filter, paging);

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

		[HttpPut("{id:long}/image")]
		[RequestSizeLimit(5_000_000)]
		public async Task<CreatureTemplateDto> UploadImage(long id, IFormFile file)
		{
			await using var stream = file.OpenReadStream();
			return await _creatureTemplateImageService.SetImageAsync(id, stream, file.ContentType, file.Length);
		}

		[HttpDelete("{id:long}/image")]
		public async Task<CreatureTemplateDto> RemoveImage(long id)
			=> await _creatureTemplateImageService.RemoveImageAsync(id);

		[HttpPut("{creatureTemplateId:long}/parts/{partId:long}/armor")]
		public async Task<CreatureTemplateDto> UpdatePartArmor(long creatureTemplateId, long partId, [FromBody] UpdateArmorPayload payload)
			=> await _creatureTemplateService.UpdatePartArmorAsync(creatureTemplateId, partId, payload.Armor);

		/// <summary>Создание/изменение навыка (upsert) — AddSkillAsync кидает на дубликат, поэтому сперва проверяем</summary>
		[HttpPut("{creatureTemplateId:long}/skills")]
		public async Task<IActionResult> UpsertSkill(long creatureTemplateId, [FromBody] CreatureTemplateSkillPayload payload)
		{
			var request = new AddOrUpdateCreatureTemplateSkillRequest { CreatureTemplateId = creatureTemplateId, Skill = payload.Skill, Value = payload.Value };

			var creatureTemplate = await _creatureTemplateService.GetCreatureTemplateByIdAsync(creatureTemplateId);
			if (creatureTemplate.Skills.ContainsKey(payload.Skill))
				await _creatureTemplateService.UpdateSkillAsync(request);
			else
				await _creatureTemplateService.AddSkillAsync(request);

			return Ok();
		}

		[HttpDelete("{creatureTemplateId:long}/skills/{skill}")]
		public async Task<IActionResult> DeleteSkill(long creatureTemplateId, Skill skill)
		{
			await _creatureTemplateService.DeleteSkillAsync(new DeleteCreatureTemplateSkillRequest { Id = creatureTemplateId, Skill = skill });
			return NoContent();
		}

		[HttpPut("{creatureTemplateId:long}/damage-type-modifiers")]
		public async Task<IActionResult> SetDamageTypeModifier(long creatureTemplateId, [FromBody] DamageTypeModifierPayload payload)
		{
			await _creatureTemplateService.SetDamageTypeModifierAsync(
				new SetDamageTypeModifierRequest { CreatureTemplateId = creatureTemplateId, DamageType = payload.DamageType, Modifier = payload.Modifier });
			return Ok();
		}

		[HttpDelete("{creatureTemplateId:long}/damage-type-modifiers/{damageType}")]
		public async Task<IActionResult> RemoveDamageTypeModifier(long creatureTemplateId, DamageType damageType)
		{
			await _creatureTemplateService.RemoveDamageTypeModifierAsync(creatureTemplateId, damageType);
			return NoContent();
		}

		[HttpPost("{creatureTemplateId:long}/abilities")]
		public async Task<CreatureTemplateDto> AddAbility(long creatureTemplateId, CreateAbilityRequest request)
		{
			request.CreatureTemplateId = creatureTemplateId;
			return await _creatureTemplateAbilityService.AddAbilityAsync(request);
		}

		[HttpPut("{creatureTemplateId:long}/abilities/{abilityId:long}")]
		public async Task<CreatureTemplateDto> UpdateAbility(long creatureTemplateId, long abilityId, UpdateAbilityRequest request)
		{
			request.CreatureTemplateId = creatureTemplateId;
			request.AbilityId = abilityId;
			return await _creatureTemplateAbilityService.UpdateAbilityAsync(request);
		}

		[HttpDelete("{creatureTemplateId:long}/abilities/{abilityId:long}")]
		public async Task<CreatureTemplateDto> RemoveAbility(long creatureTemplateId, long abilityId)
			=> await _creatureTemplateAbilityService.RemoveAbilityAsync(creatureTemplateId, abilityId);

		[HttpPost("{creatureTemplateId:long}/abilities/{abilityId:long}/conditions")]
		public async Task<CreatureTemplateDto> AddCondition(long creatureTemplateId, long abilityId, AddAbilityConditionRequest request)
		{
			request.CreatureTemplateId = creatureTemplateId;
			request.AbilityId = abilityId;
			return await _creatureTemplateAbilityService.AddAbilityConditionAsync(request);
		}

		[HttpPut("{creatureTemplateId:long}/abilities/{abilityId:long}/conditions/{conditionId:long}")]
		public async Task<CreatureTemplateDto> UpdateCondition(long creatureTemplateId, long abilityId, long conditionId, UpdateAbilityConditionRequest request)
		{
			request.CreatureTemplateId = creatureTemplateId;
			request.AbilityId = abilityId;
			request.ConditionId = conditionId;
			return await _creatureTemplateAbilityService.UpdateAbilityConditionAsync(request);
		}

		[HttpDelete("{creatureTemplateId:long}/abilities/{abilityId:long}/conditions/{conditionId:long}")]
		public async Task<CreatureTemplateDto> RemoveCondition(long creatureTemplateId, long abilityId, long conditionId)
			=> await _creatureTemplateAbilityService.RemoveAbilityConditionAsync(creatureTemplateId, abilityId, conditionId);

		[HttpPost("{creatureTemplateId:long}/abilities/{abilityId:long}/defensive-skills")]
		public async Task<CreatureTemplateDto> AddDefensiveSkill(long creatureTemplateId, long abilityId, AddAbilityDefensiveSkillRequest request)
		{
			request.CreatureTemplateId = creatureTemplateId;
			request.AbilityId = abilityId;
			return await _creatureTemplateAbilityService.AddAbilityDefensiveSkillAsync(request);
		}

		[HttpDelete("{creatureTemplateId:long}/abilities/{abilityId:long}/defensive-skills/{defensiveSkillId:long}")]
		public async Task<CreatureTemplateDto> RemoveDefensiveSkill(long creatureTemplateId, long abilityId, long defensiveSkillId)
			=> await _creatureTemplateAbilityService.RemoveAbilityDefensiveSkillAsync(creatureTemplateId, abilityId, defensiveSkillId);
	}

	public sealed class UpdateArmorPayload
	{
		public int Armor { get; set; }
	}

	public sealed class CreatureTemplateSkillPayload
	{
		public Skill Skill { get; set; }
		public int Value { get; set; }
	}

	public sealed class DamageTypeModifierPayload
	{
		public DamageType DamageType { get; set; }
		public DamageTypeModifier Modifier { get; set; }
	}
}
