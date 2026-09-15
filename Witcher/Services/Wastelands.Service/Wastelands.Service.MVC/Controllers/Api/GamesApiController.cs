using Microsoft.AspNetCore.Mvc;
using Wastelands.Service.Domain.Contracts;
using Wastelands.Service.Domain.Models.Dto;
using Wastelands.Service.Domain.Models.Filters;
using Wastelands.Service.Domain.Models.Requests;

namespace Wastelands.Service.MVC.Controllers.Api
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
		public async Task<List<GameDto>> Index([FromQuery] GameFilter filter)
			=> await _gameService.GetGamesAsync(filter);

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
	}
}
