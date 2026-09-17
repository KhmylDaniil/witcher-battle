using Wastelands.Service.Application.Models.Dto;
using Wastelands.Service.Application.Models.Filters;
using Wastelands.Service.Application.Models.Requests;

namespace Wastelands.Service.Application.Contracts
{
	/// <summary>Лист персонажа и навыки. Способности — в ICharacterAbilityService.</summary>
	public interface ICharacterService
	{
		Task<CharacterDto> GetCharacterByIdAsync(long id);

		Task<List<CharacterDto>> GetCharactersAsync(CharacterFilter filter);

		/// <summary>Персонажи всех игроков этой игры — доступно только мастеру игры.</summary>
		Task<List<CharacterDto>> GetGameCharactersAsync(long gameId);

		Task<CharacterDto> CreateCharacterAsync(CreateCharacterRequest request);

		Task<CharacterDto> UpdateCharacterAsync(UpdateCharacterRequest request);

		Task AddSkillAsync(AddOrUpdateCharacterSkillRequest request);

		Task UpdateSkillAsync(AddOrUpdateCharacterSkillRequest request);

		Task DeleteCharacterAsync(long id);

		Task DeleteSkillAsync(DeleteCharacterSkillRequest request);
	}
}
