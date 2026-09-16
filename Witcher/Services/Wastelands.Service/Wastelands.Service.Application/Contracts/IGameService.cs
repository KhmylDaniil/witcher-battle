using Wastelands.Service.Application.Models.Dto;
using Wastelands.Service.Application.Models.Filters;
using Wastelands.Service.Application.Models.Requests;

namespace Wastelands.Service.Application.Contracts
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
