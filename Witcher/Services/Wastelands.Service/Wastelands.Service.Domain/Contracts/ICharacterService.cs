using Wastelands.Service.Domain.Models.Dto;
using Wastelands.Service.Domain.Models.Filters;
using Wastelands.Service.Domain.Models.Requests;

namespace Wastelands.Service.Domain.Contracts
{
	public interface ICharacterService
	{
		Task<CharacterDto> GetCharacterByIdAsync(long id);

		Task<List<CharacterDto>> GetCharactersAsync(CharacterFilter filter);

		Task<CharacterDto> CreateCharacterAsync(CreateCharacterRequest request);

		Task<CharacterDto> UpdateCharacterAsync(UpdateCharacterRequest request);

		Task DeleteCharacterAsync(long id);
	}
}
