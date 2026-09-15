using Wastelands.Service.Domain.Models.Dto;
using Wastelands.Service.Domain.Models.Filters;
using Wastelands.Service.Domain.Models.Requests;

namespace Wastelands.Service.Domain.Contracts
{
	public interface IGameService
	{
		Task<GameDto> GetGameByIdAsync(long id);

		Task<List<GameDto>> GetGamesAsync(GameFilter filter);

		Task<List<GameDto>> GetMyGamesAsync();

		Task<GameDto> CreateGameAsync(CreateGameRequest request);

		Task<GameDto> UpdateGameAsync(UpdateGameRequest request);

		Task DeleteGameAsync(long id);
	}
}
