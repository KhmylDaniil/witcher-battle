using Wastelands.Core.Contracts.Models;
using Wastelands.Service.Application.Models.Dto;
using Wastelands.Service.Application.Models.Filters;
using Wastelands.Service.Application.Models.Requests;

namespace Wastelands.Service.Application.Contracts
{
	public interface IGameService
	{
		Task<GameDto> GetGameByIdAsync(long id);

		Task<PagedResultDto<GameDto>> GetGamesAsync(GameFilter filter, PagedRequest paging);

		Task<List<GameDto>> GetMyGamesAsync();

		Task<GameDto> CreateGameAsync(CreateGameRequest request);

		Task<GameDto> UpdateGameAsync(UpdateGameRequest request);

		Task DeleteGameAsync(long id);
	}
}
