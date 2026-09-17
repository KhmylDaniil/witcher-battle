using Wastelands.Service.Application.Models.Dto;

namespace Wastelands.Service.Application.Contracts
{
	/// <summary>
	/// Полная read-only карточка участника боя (изображение, статы, части тела, способности) —
	/// доступна любому, кто видит сам бой (см. BattleRepository.GetQuery()), в отличие от
	/// ICreatureTemplateService/ICharacterService, которые ограничены владельцем ресурса.
	/// </summary>
	public interface IBattleParticipantSheetService
	{
		Task<CreatureTemplateDto> GetCreatureSheetAsync(long battleId, long creatureId);

		Task<CharacterDto> GetCharacterSheetAsync(long battleId, long characterId);
	}
}
