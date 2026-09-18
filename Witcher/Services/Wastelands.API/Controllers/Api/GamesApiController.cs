using Microsoft.AspNetCore.Mvc;
using Wastelands.Core.Contracts.Models;
using Wastelands.Service.Application.Contracts;
using Wastelands.Service.Application.Models.Dto;
using Wastelands.Service.Application.Models.Filters;
using Wastelands.Service.Application.Models.Requests;

namespace Wastelands.API.Controllers.Api
{
	[Route("api/games")]
	public class GamesApiController : ApiControllerBase
	{
		private readonly IGameService _gameService;

		public GamesApiController(IGameService gameService)
		{
			_gameService = gameService;
		}

		[HttpGet]
		public async Task<PagedResultDto<GameDto>> Index([FromQuery] GameFilter filter, [FromQuery] PagedRequest paging)
			=> await _gameService.GetGamesAsync(filter, paging);

		[HttpGet("mine")]
		public async Task<List<GameDto>> Mine()
			=> await _gameService.GetMyGamesAsync();

		[HttpGet("{id:long}")]
		public async Task<GameDto> Get(long id)
			=> await _gameService.GetGameByIdAsync(id);

		[HttpPost]
		public async Task<GameDto> Create(CreateGameRequest request)
			=> await _gameService.CreateGameAsync(request);

		[HttpPut("{id:long}")]
		public async Task<GameDto> Update(long id, UpdateGameRequest request)
		{
			request.Id = id;
			return await _gameService.UpdateGameAsync(request);
		}

		[HttpDelete("{id:long}")]
		public async Task<IActionResult> Delete(long id)
		{
			await _gameService.DeleteGameAsync(id);
			return NoContent();
		}

		[HttpGet("{id:long}/members")]
		public async Task<List<long>> Members(long id)
			=> await _gameService.GetMemberUserIdsAsync(id);

		[HttpDelete("{id:long}/members/{userId:long}")]
		public async Task<IActionResult> RemoveMember(long id, long userId)
		{
			await _gameService.RemoveMemberAsync(id, userId);
			return NoContent();
		}
	}
}
