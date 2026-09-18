using Wastelands.Service.Application.Models.Dto;
using Wastelands.Service.Application.Models.Requests;

namespace Wastelands.Service.Application.Contracts
{
	/// <summary>
	/// Инвентарь персонажа — добавление/удаление экземпляров предметов и их экипировка. Три разных
	/// уровня доступа: AddItemAsync — только мастер игры персонажа; EquipAsync/UnequipAsync — только
	/// владелец персонажа; RemoveItemAsync — мастер или владелец.
	/// </summary>
	public interface ICharacterItemService
	{
		Task<CharacterDto> AddItemAsync(AddItemRequest request);

		Task<CharacterDto> RemoveItemAsync(long characterId, long itemId);

		Task<CharacterDto> EquipAsync(long characterId, long itemId);

		Task<CharacterDto> UnequipAsync(long characterId, long itemId);

		/// <summary>Изменение прочности экземпляра оружия/брони (ремонт) — только мастер игры персонажа.</summary>
		Task<CharacterDto> RepairAsync(RepairItemRequest request);
	}
}
