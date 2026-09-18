using Microsoft.AspNetCore.Mvc;
using Wastelands.Core.Contracts.Models;
using Wastelands.Service.Application.Contracts;
using Wastelands.Service.Application.Models.Dto;
using Wastelands.Service.Application.Models.Filters;
using Wastelands.Service.Application.Models.Requests;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.API.Controllers.Api
{
	/// <summary>Создание и редактирование шаблонов предметов — доступно только мастеру игры (см. ItemTemplateService/ItemTemplateRepository).</summary>
	[Route("api/item-templates")]
	public class ItemTemplatesApiController : ApiControllerBase
	{
		private readonly IItemTemplateService _itemTemplateService;

		public ItemTemplatesApiController(IItemTemplateService itemTemplateService)
		{
			_itemTemplateService = itemTemplateService;
		}

		[HttpGet]
		public async Task<PagedResultDto<ItemTemplateDto>> Index([FromQuery] ItemTemplateFilter filter, [FromQuery] PagedRequest paging)
			=> await _itemTemplateService.GetItemTemplatesAsync(filter, paging);

		[HttpGet("{id:long}")]
		public async Task<ItemTemplateDto> Get(long id)
			=> await _itemTemplateService.GetItemTemplateByIdAsync(id);

		[HttpPost]
		public async Task<ItemTemplateDto> Create(CreateItemTemplateRequest request)
			=> await _itemTemplateService.CreateItemTemplateAsync(request);

		[HttpPut("{id:long}")]
		public async Task<ItemTemplateDto> Update(long id, UpdateItemTemplateRequest request)
		{
			request.Id = id;
			return await _itemTemplateService.UpdateItemTemplateAsync(request);
		}

		[HttpDelete("{id:long}")]
		public async Task<IActionResult> Delete(long id)
		{
			await _itemTemplateService.DeleteItemTemplateAsync(id);
			return NoContent();
		}

		[HttpPost("{itemTemplateId:long}/applied-conditions")]
		public async Task<ItemTemplateDto> AddCondition(long itemTemplateId, AddItemTemplateConditionRequest request)
		{
			request.ItemTemplateId = itemTemplateId;
			return await _itemTemplateService.AddConditionAsync(request);
		}

		[HttpPut("{itemTemplateId:long}/applied-conditions/{conditionId:long}")]
		public async Task<ItemTemplateDto> UpdateCondition(long itemTemplateId, long conditionId, UpdateItemTemplateConditionRequest request)
		{
			request.ItemTemplateId = itemTemplateId;
			request.ConditionId = conditionId;
			return await _itemTemplateService.UpdateConditionAsync(request);
		}

		[HttpDelete("{itemTemplateId:long}/applied-conditions/{conditionId:long}")]
		public async Task<ItemTemplateDto> RemoveCondition(long itemTemplateId, long conditionId)
			=> await _itemTemplateService.RemoveConditionAsync(itemTemplateId, conditionId);

		[HttpPost("{itemTemplateId:long}/armor-parts")]
		public async Task<ItemTemplateDto> AddArmorPart(long itemTemplateId, ArmorPartPayload payload)
			=> await _itemTemplateService.AddArmorPartAsync(new AddItemTemplateArmorPartRequest
			{
				ItemTemplateId = itemTemplateId,
				Part = payload.Part,
				ArmorValue = payload.ArmorValue,
				MaxDurability = payload.MaxDurability,
			});

		[HttpPut("{itemTemplateId:long}/armor-parts/{armorPartId:long}")]
		public async Task<ItemTemplateDto> UpdateArmorPart(long itemTemplateId, long armorPartId, ArmorPartPayload payload)
			=> await _itemTemplateService.UpdateArmorPartAsync(new UpdateItemTemplateArmorPartRequest
			{
				ItemTemplateId = itemTemplateId,
				ArmorPartId = armorPartId,
				ArmorValue = payload.ArmorValue,
				MaxDurability = payload.MaxDurability,
			});

		[HttpDelete("{itemTemplateId:long}/armor-parts/{armorPartId:long}")]
		public async Task<ItemTemplateDto> RemoveArmorPart(long itemTemplateId, long armorPartId)
			=> await _itemTemplateService.RemoveArmorPartAsync(itemTemplateId, armorPartId);

		[HttpPut("{itemTemplateId:long}/damage-type-modifiers")]
		public async Task<ItemTemplateDto> SetDamageTypeModifier(long itemTemplateId, DamageTypeModifierPayload payload)
			=> await _itemTemplateService.SetDamageTypeModifierAsync(new SetItemTemplateDamageTypeModifierRequest
			{
				ItemTemplateId = itemTemplateId,
				DamageType = payload.DamageType,
				Modifier = payload.Modifier,
			});

		[HttpDelete("{itemTemplateId:long}/damage-type-modifiers/{damageType}")]
		public async Task<ItemTemplateDto> RemoveDamageTypeModifier(long itemTemplateId, DamageType damageType)
			=> await _itemTemplateService.RemoveDamageTypeModifierAsync(itemTemplateId, damageType);
	}

	public sealed class ArmorPartPayload
	{
		public HumanBodyPart Part { get; set; }

		public int ArmorValue { get; set; }

		public int MaxDurability { get; set; }
	}
}
