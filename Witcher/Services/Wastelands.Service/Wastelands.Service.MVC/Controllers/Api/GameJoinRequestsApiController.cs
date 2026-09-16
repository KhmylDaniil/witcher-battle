using Microsoft.AspNetCore.Mvc;
using Wastelands.Service.Application.Contracts;
using Wastelands.Service.Application.Models.Dto;

namespace Wastelands.Service.MVC.Controllers.Api
{
	[Route("api/games/{gameId:long}/join-requests")]
	public class GameJoinRequestsApiController : ApiControllerBase
	{
		private readonly IGameJoinRequestService _gameJoinRequestService;

		public GameJoinRequestsApiController(IGameJoinRequestService gameJoinRequestService)
		{
			_gameJoinRequestService = gameJoinRequestService;
		}

		[HttpPost]
		public async Task<GameJoinRequestDto> Create(long gameId)
			=> await _gameJoinRequestService.CreateJoinRequestAsync(gameId);

		/// <summary>Входящие заявки (только Pending) — доступно только создателю игры.</summary>
		[HttpGet]
		public async Task<List<GameJoinRequestDto>> Incoming(long gameId)
			=> await _gameJoinRequestService.GetIncomingJoinRequestsAsync(gameId);

		[HttpPost("{requestId:long}/accept")]
		public async Task<IActionResult> Accept(long gameId, long requestId)
		{
			await _gameJoinRequestService.AcceptJoinRequestAsync(requestId);
			return Ok();
		}

		[HttpPost("{requestId:long}/decline")]
		public async Task<IActionResult> Decline(long gameId, long requestId)
		{
			await _gameJoinRequestService.DeclineJoinRequestAsync(requestId);
			return Ok();
		}

		/// <summary>Мои заявки на присоединение, по всем играм.</summary>
		[HttpGet("/api/join-requests/mine")]
		public async Task<List<GameJoinRequestDto>> Mine()
			=> await _gameJoinRequestService.GetMyJoinRequestsAsync();
	}
}
