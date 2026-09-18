using Wastelands.Core.Contracts.Contracts;
using Wastelands.Core.Contracts.Enums;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Service.Application.Contracts;
using Wastelands.Service.Application.Contracts.Repositories;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Application.Services
{
	public class GameAccessGuard : IGameAccessGuard
	{
		private readonly IGameRepository _gameRepository;
		private readonly IUserContext _userContext;

		public GameAccessGuard(IGameRepository gameRepository, IUserContext userContext)
		{
			_gameRepository = gameRepository;
			_userContext = userContext;
		}

		public async Task<Game> GetGameOwnedByCurrentUserAsync(long gameId)
		{
			var game = await _gameRepository.GetByIdAsync(gameId);
			NotFoundException.ThrowIfNull(game, ErrorCode.GameNotFound, nameof(Game), nameof(Game.Id), gameId.ToString());

			EnsureOwner(game);

			return game;
		}

		public void EnsureOwner(Game game)
		{
			if (game.CreatedByUserId != _userContext.CurrentUserId)
			{
				throw new InvalidArgumentException(
					ErrorCode.CurrentUserNotAllowedToPerformThisAction,
					"Только создатель игры может выполнить это действие.");
			}
		}
	}
}
