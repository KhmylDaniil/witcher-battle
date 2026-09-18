using Wastelands.Core.Contracts.Models;
using Wastelands.Service.Application.Models.Dto;
using Wastelands.Service.Application.Models.Filters;
using Wastelands.Service.Application.Models.Requests;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Contracts
{
	/// <summary>Шаблоны предметов игры — доступны только мастеру (GM-скоуп в ItemTemplateRepository).</summary>
	public interface IItemTemplateService
	{
		Task<ItemTemplateDto> GetItemTemplateByIdAsync(long id);

		Task<PagedResultDto<ItemTemplateDto>> GetItemTemplatesAsync(ItemTemplateFilter filter, PagedRequest paging);

		Task<ItemTemplateDto> CreateItemTemplateAsync(CreateItemTemplateRequest request);

		Task<ItemTemplateDto> UpdateItemTemplateAsync(UpdateItemTemplateRequest request);

		Task DeleteItemTemplateAsync(long id);

		Task<ItemTemplateDto> AddConditionAsync(AddItemTemplateConditionRequest request);

		Task<ItemTemplateDto> UpdateConditionAsync(UpdateItemTemplateConditionRequest request);

		Task<ItemTemplateDto> RemoveConditionAsync(long itemTemplateId, long conditionId);

		Task<ItemTemplateDto> AddArmorPartAsync(AddItemTemplateArmorPartRequest request);

		Task<ItemTemplateDto> UpdateArmorPartAsync(UpdateItemTemplateArmorPartRequest request);

		Task<ItemTemplateDto> RemoveArmorPartAsync(long itemTemplateId, long armorPartId);

		Task<ItemTemplateDto> SetDamageTypeModifierAsync(SetItemTemplateDamageTypeModifierRequest request);

		Task<ItemTemplateDto> RemoveDamageTypeModifierAsync(long itemTemplateId, DamageType damageType);
	}
}
