using AutoMapper;
using Wastelands.Core.Contracts.Contracts;
using Wastelands.Core.Contracts.Enums;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Service.Application.Contracts;
using Wastelands.Service.Application.Contracts.Repositories;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;
using Wastelands.Service.Application.Models.Dto;
using Wastelands.Service.Application.Models.Requests;

namespace Wastelands.Service.Application.Services
{
	public class CharacterItemService : ICharacterItemService
	{
		private readonly ICharacterRepository _characterRepository;
		private readonly IItemTemplateRepository _itemTemplateRepository;
		private readonly IGameRepository _gameRepository;
		private readonly IUserContext _userContext;
		private readonly IMapper _mapper;

		public CharacterItemService(
			ICharacterRepository characterRepository,
			IItemTemplateRepository itemTemplateRepository,
			IGameRepository gameRepository,
			IUserContext userContext,
			IMapper mapper)
		{
			_characterRepository = characterRepository;
			_itemTemplateRepository = itemTemplateRepository;
			_gameRepository = gameRepository;
			_userContext = userContext;
			_mapper = mapper;
		}

		public async Task<CharacterDto> AddItemAsync(AddItemRequest request)
		{
			var character = await GetForGmMutationAsync(request.CharacterId);

			var itemTemplate = await _itemTemplateRepository.GetByIdAsync(request.ItemTemplateId);
			NotFoundException.ThrowIfNull(
				itemTemplate, ErrorCode.ItemTemplateNotFound, nameof(ItemTemplate), nameof(ItemTemplate.Id), request.ItemTemplateId.ToString());

			if (itemTemplate.GameId != character.GameId)
			{
				throw new InvalidArgumentException(ErrorCode.ItemTemplateBelongsToAnotherGame, "Выбранный шаблон предмета принадлежит другой игре.");
			}

			character.Items.Add(new Item(character.Id, itemTemplate));
			await _characterRepository.UpdateAsync(character);

			return _mapper.Map<CharacterDto>(character);
		}

		public async Task<CharacterDto> RemoveItemAsync(long characterId, long itemId)
		{
			var character = await GetForGmOrOwnerMutationAsync(characterId);
			var item = GetItem(character, itemId);

			if (item.IsEquipped)
			{
				RemoveGeneratedAbilities(character, item);
			}

			character.Items.Remove(item);
			await _characterRepository.UpdateAsync(character);

			return _mapper.Map<CharacterDto>(character);
		}

		public async Task<CharacterDto> EquipAsync(long characterId, long itemId)
		{
			var character = await GetForOwnerMutationAsync(characterId);
			var item = GetItem(character, itemId);

			item.Equip();

			if (item.ItemType == ItemType.Weapon)
			{
				GenerateWeaponAbilities(character, item);
			}

			await _characterRepository.UpdateAsync(character);

			return _mapper.Map<CharacterDto>(character);
		}

		public async Task<CharacterDto> UnequipAsync(long characterId, long itemId)
		{
			var character = await GetForOwnerMutationAsync(characterId);
			var item = GetItem(character, itemId);

			item.Unequip();
			RemoveGeneratedAbilities(character, item);

			await _characterRepository.UpdateAsync(character);

			return _mapper.Map<CharacterDto>(character);
		}

		/// <summary>
		/// Оружие без мультиатаки (IsMultiAttack == false на предмете) даёт одну способность с именем
		/// оружия и скоростью 1; с мультиатакой (IsMultiAttack == true) — «Быструю атаку» (скорость 2,
		/// урон как в предмете) и «Сильную атаку» (скорость 1, урон ×2 кубиками, модификатор -3).
		/// </summary>
		private static void GenerateWeaponAbilities(Character character, Item item)
		{
			var appliedConditions = item.AppliedConditions.Select(c => (c.Condition, c.ApplyChance)).ToList();

			if (item.IsMultiAttack == true)
			{
				character.Abilities.Add(Ability.ForEquippedWeapon(
					character.Id, item.Id, $"Быстрая атака ({item.Name})", item.AttackSkill!.Value, attacksPerTurn: 2,
					item.DamageDiceCount!.Value, item.DamageModifier!.Value, item.DamageType!.Value, appliedConditions));

				character.Abilities.Add(Ability.ForEquippedWeapon(
					character.Id, item.Id, $"Сильная атака ({item.Name})", item.AttackSkill!.Value, attacksPerTurn: 1,
					item.DamageDiceCount!.Value * 2, item.DamageModifier!.Value - 3, item.DamageType!.Value, appliedConditions));
			}
			else
			{
				character.Abilities.Add(Ability.ForEquippedWeapon(
					character.Id, item.Id, item.Name, item.AttackSkill!.Value, attacksPerTurn: 1,
					item.DamageDiceCount!.Value, item.DamageModifier!.Value, item.DamageType!.Value, appliedConditions));
			}
		}

		private static void RemoveGeneratedAbilities(Character character, Item item)
		{
			foreach (var ability in character.Abilities.Where(a => a.EquippedItemId == item.Id).ToList())
			{
				character.Abilities.Remove(ability);
			}
		}

		private static Item GetItem(Character character, long itemId)
		{
			var item = character.Items.FirstOrDefault(x => x.Id == itemId);
			NotFoundException.ThrowIfNull(item, ErrorCode.ItemNotFound, nameof(Item), nameof(Item.Id), itemId.ToString());

			return item;
		}

		/// <summary>Только мастер игры персонажа — копия CharacterService.GetGameCharactersAsync.</summary>
		private async Task<Character> GetForGmMutationAsync(long characterId)
		{
			var character = await _characterRepository.GetByIdUnscopedAsync(characterId);
			NotFoundException.ThrowIfNull(character, ErrorCode.CharacterNotFound, nameof(Character), nameof(Character.Id), characterId.ToString());

			if (character.GameId is not { } gameId)
			{
				throw new InvalidArgumentException(
					ErrorCode.CurrentUserNotAllowedToPerformThisAction, "У архивного персонажа нет мастера, который мог бы выдавать предметы.");
			}

			var game = await _gameRepository.GetByIdAsync(gameId);
			NotFoundException.ThrowIfNull(game, ErrorCode.GameNotFound, nameof(Game), nameof(Game.Id), gameId.ToString());

			if (game.CreatedByUserId != _userContext.CurrentUserId)
			{
				throw new InvalidArgumentException(
					ErrorCode.CurrentUserNotAllowedToPerformThisAction, "Добавлять предметы персонажу может только мастер его игры.");
			}

			return character;
		}

		/// <summary>Только владелец персонажа — обычный скоуп ICharacterRepository.GetByIdAsync.</summary>
		private async Task<Character> GetForOwnerMutationAsync(long characterId)
		{
			var character = await _characterRepository.GetByIdAsync(characterId);
			NotFoundException.ThrowIfNull(character, ErrorCode.CharacterNotFound, nameof(Character), nameof(Character.Id), characterId.ToString());

			return character;
		}

		/// <summary>Мастер игры ИЛИ владелец — копия CharacterService.GetCharacterByIdAsync.</summary>
		private async Task<Character> GetForGmOrOwnerMutationAsync(long characterId)
		{
			var character = await _characterRepository.GetByIdAsync(characterId) ?? await GetIfCurrentUserIsGameMasterAsync(characterId);
			NotFoundException.ThrowIfNull(character, ErrorCode.CharacterNotFound, nameof(Character), nameof(Character.Id), characterId.ToString());

			return character;
		}

		private async Task<Character?> GetIfCurrentUserIsGameMasterAsync(long characterId)
		{
			var character = await _characterRepository.GetByIdUnscopedAsync(characterId);
			if (character?.GameId is not { } gameId)
			{
				return null;
			}

			var game = await _gameRepository.GetByIdAsync(gameId);
			return game?.CreatedByUserId == _userContext.CurrentUserId ? character : null;
		}
	}
}
