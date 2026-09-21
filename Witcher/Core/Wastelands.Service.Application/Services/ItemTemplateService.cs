using AutoMapper;
using Wastelands.Core.Contracts.Constants;
using Wastelands.Core.Contracts.Enums;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Core.Contracts.Models;
using Wastelands.Service.Application.Contracts;
using Wastelands.Service.Application.Contracts.Repositories;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;
using Wastelands.Service.Application.Models.Dto;
using Wastelands.Service.Application.Models.Filters;
using Wastelands.Service.Application.Models.Requests;

namespace Wastelands.Service.Application.Services
{
	/// <summary>CRUD шаблонов предметов и их накладываемых состояний (только для ItemType == Weapon).</summary>
	public class ItemTemplateService : IItemTemplateService
	{
		private readonly IItemTemplateRepository _itemTemplateRepository;
		private readonly IGameAccessGuard _gameAccessGuard;
		private readonly IMapper _mapper;

		public ItemTemplateService(IItemTemplateRepository itemTemplateRepository, IGameAccessGuard gameAccessGuard, IMapper mapper)
		{
			_itemTemplateRepository = itemTemplateRepository;
			_gameAccessGuard = gameAccessGuard;
			_mapper = mapper;
		}

		public async Task<ItemTemplateDto> GetItemTemplateByIdAsync(long id)
		{
			var itemTemplate = await GetByIdAsync(id);
			return _mapper.Map<ItemTemplateDto>(itemTemplate);
		}

		public async Task<PagedResultDto<ItemTemplateDto>> GetItemTemplatesAsync(ItemTemplateFilter filter, PagedRequest paging)
		{
			var paged = await _itemTemplateRepository.GetPagedAsync(paging, filter);
			var dtos = _mapper.Map<List<ItemTemplateDto>>(paged.Entities);

			return new PagedResultDto<ItemTemplateDto> { Items = dtos, TotalCount = paged.TotalCount, PageNumber = paging.PageNumber, PageSize = paging.PageSize };
		}

		public async Task<ItemTemplateDto> CreateItemTemplateAsync(CreateItemTemplateRequest request)
		{
			var game = await _gameAccessGuard.GetGameOwnedByCurrentUserAsync(request.GameId);

			var entity = request.ItemType == ItemType.Weapon
				? ItemTemplate.CreateWeapon(
					game.Id, request.Name, request.Description, request.Weight, request.Cost,
					request.AttackSkill!.Value, request.IsMultiAttack!.Value, request.DamageDiceCount!.Value,
					request.AttackModifier!.Value, request.DamageModifier!.Value, request.DamageType!.Value, request.WeaponKind!.Value,
					request.AttackRange!.Value, request.HandsRequired!.Value, request.Durability!.Value)
				: ItemTemplate.CreateNonWeapon(game.Id, request.Name, request.Description, request.ItemType, request.Weight, request.Cost);

			await _itemTemplateRepository.CreateAsync(entity);

			return _mapper.Map<ItemTemplateDto>(entity);
		}

		public async Task<ItemTemplateDto> UpdateItemTemplateAsync(UpdateItemTemplateRequest request)
		{
			var itemTemplate = await GetByIdAsync(request.Id);

			if (request.ItemType == ItemType.Weapon)
			{
				itemTemplate.ChangeWeapon(
					request.Name, request.Description, request.Weight, request.Cost,
					request.AttackSkill!.Value, request.IsMultiAttack!.Value, request.DamageDiceCount!.Value,
					request.AttackModifier!.Value, request.DamageModifier!.Value, request.DamageType!.Value, request.WeaponKind!.Value,
					request.AttackRange!.Value, request.HandsRequired!.Value, request.Durability!.Value);
			}
			else
			{
				itemTemplate.ChangeNonWeapon(request.Name, request.Description, request.Weight, request.Cost);
			}

			await _itemTemplateRepository.UpdateAsync(itemTemplate);

			return _mapper.Map<ItemTemplateDto>(itemTemplate);
		}

		public async Task DeleteItemTemplateAsync(long id)
		{
			var itemTemplate = await GetByIdAsync(id);
			await _itemTemplateRepository.DeleteAsync(itemTemplate);
		}

		public async Task<ItemTemplateDto> AddConditionAsync(AddItemTemplateConditionRequest request)
		{
			var itemTemplate = await GetByIdAsync(request.ItemTemplateId);
			EnsureWeapon(itemTemplate);
			ThrowIfConditionDuplicate(itemTemplate, request.Condition, excludeConditionId: null);

			itemTemplate.AppliedConditions.Add(new ItemTemplateAppliedCondition(itemTemplate.Id, request.Condition, request.ApplyChance));
			await _itemTemplateRepository.UpdateAsync(itemTemplate);

			return _mapper.Map<ItemTemplateDto>(itemTemplate);
		}

		public async Task<ItemTemplateDto> UpdateConditionAsync(UpdateItemTemplateConditionRequest request)
		{
			var itemTemplate = await GetByIdAsync(request.ItemTemplateId);
			var condition = GetCondition(itemTemplate, request.ConditionId);

			ThrowIfConditionDuplicate(itemTemplate, request.Condition, excludeConditionId: request.ConditionId);

			condition.ChangeCondition(request.Condition, request.ApplyChance);
			await _itemTemplateRepository.UpdateAsync(itemTemplate);

			return _mapper.Map<ItemTemplateDto>(itemTemplate);
		}

		public async Task<ItemTemplateDto> RemoveConditionAsync(long itemTemplateId, long conditionId)
		{
			var itemTemplate = await GetByIdAsync(itemTemplateId);
			var condition = GetCondition(itemTemplate, conditionId);

			itemTemplate.AppliedConditions.Remove(condition);
			await _itemTemplateRepository.UpdateAsync(itemTemplate);

			return _mapper.Map<ItemTemplateDto>(itemTemplate);
		}

		public async Task<ItemTemplateDto> AddArmorPartAsync(AddItemTemplateArmorPartRequest request)
		{
			var itemTemplate = await GetByIdAsync(request.ItemTemplateId);
			EnsureArmor(itemTemplate);

			itemTemplate.AddArmorPart(request.Part, request.Armor);
			await _itemTemplateRepository.UpdateAsync(itemTemplate);

			return _mapper.Map<ItemTemplateDto>(itemTemplate);
		}

		public async Task<ItemTemplateDto> UpdateArmorPartAsync(UpdateItemTemplateArmorPartRequest request)
		{
			var itemTemplate = await GetByIdAsync(request.ItemTemplateId);
			var armorPart = GetArmorPart(itemTemplate, request.ArmorPartId);

			armorPart.Change(request.Armor);
			await _itemTemplateRepository.UpdateAsync(itemTemplate);

			return _mapper.Map<ItemTemplateDto>(itemTemplate);
		}

		public async Task<ItemTemplateDto> RemoveArmorPartAsync(long itemTemplateId, long armorPartId)
		{
			var itemTemplate = await GetByIdAsync(itemTemplateId);
			var armorPart = GetArmorPart(itemTemplate, armorPartId);

			itemTemplate.ArmorParts.Remove(armorPart);
			await _itemTemplateRepository.UpdateAsync(itemTemplate);

			return _mapper.Map<ItemTemplateDto>(itemTemplate);
		}

		public async Task<ItemTemplateDto> SetDamageTypeModifierAsync(SetItemTemplateDamageTypeModifierRequest request)
		{
			var itemTemplate = await GetByIdAsync(request.ItemTemplateId);
			EnsureArmor(itemTemplate);

			itemTemplate.DamageTypeModifiers[request.DamageType] = request.Modifier;
			await _itemTemplateRepository.UpdateAsync(itemTemplate);

			return _mapper.Map<ItemTemplateDto>(itemTemplate);
		}

		public async Task<ItemTemplateDto> RemoveDamageTypeModifierAsync(long itemTemplateId, DamageType damageType)
		{
			var itemTemplate = await GetByIdAsync(itemTemplateId);

			itemTemplate.DamageTypeModifiers.Remove(damageType);
			await _itemTemplateRepository.UpdateAsync(itemTemplate);

			return _mapper.Map<ItemTemplateDto>(itemTemplate);
		}

		private static void EnsureWeapon(ItemTemplate itemTemplate)
		{
			if (itemTemplate.ItemType != ItemType.Weapon)
			{
				throw new InvalidArgumentException(ErrorCode.ItemTemplateNotWeapon, "Накладываемые состояния можно добавлять только шаблонам оружия.");
			}
		}

		private static void EnsureArmor(ItemTemplate itemTemplate)
		{
			if (itemTemplate.ItemType != ItemType.Armor)
			{
				throw new InvalidArgumentException(ErrorCode.ItemTemplateNotArmor, "Это доступно только шаблонам брони.");
			}
		}

		private static ItemTemplateArmorPart GetArmorPart(ItemTemplate itemTemplate, long armorPartId)
		{
			var armorPart = itemTemplate.ArmorParts.FirstOrDefault(x => x.Id == armorPartId);

			NotFoundException.ThrowIfNull(
				armorPart, ErrorCode.ItemTemplateArmorPartNotFound, nameof(ItemTemplateArmorPart), nameof(ItemTemplateArmorPart.Id), armorPartId.ToString());

			return armorPart;
		}

		private static ItemTemplateAppliedCondition GetCondition(ItemTemplate itemTemplate, long conditionId)
		{
			var condition = itemTemplate.AppliedConditions.FirstOrDefault(x => x.Id == conditionId);

			NotFoundException.ThrowIfNull(
				condition, ErrorCode.ItemTemplateConditionNotFound, nameof(ItemTemplateAppliedCondition), nameof(ItemTemplateAppliedCondition.Id), conditionId.ToString());

			return condition;
		}

		private static void ThrowIfConditionDuplicate(ItemTemplate itemTemplate, Condition condition, long? excludeConditionId)
		{
			if (itemTemplate.AppliedConditions.Any(x => x.Condition == condition && x.Id != excludeConditionId))
			{
				throw new InvalidArgumentException(
					ErrorCode.ItemTemplateConditionAlreadyExisted,
					string.Format(ExceptionMessages.ValueMustBeUnique, nameof(condition)));
			}
		}

		private async Task<ItemTemplate> GetByIdAsync(long id)
		{
			var itemTemplate = await _itemTemplateRepository.GetByIdAsync(id);

			NotFoundException.ThrowIfNull(
				itemTemplate, ErrorCode.ItemTemplateNotFound, nameof(ItemTemplate), nameof(ItemTemplate.Id), id.ToString());

			return itemTemplate;
		}
	}
}
