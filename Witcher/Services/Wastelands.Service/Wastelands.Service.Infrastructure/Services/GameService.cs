using AutoMapper;
using Wastelands.Core.Contracts.Contracts;
using Wastelands.Core.Contracts.Enums;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Service.Domain.Contracts;
using Wastelands.Service.Domain.Contracts.Repositories;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;
using Wastelands.Service.Domain.Models.Dto;
using Wastelands.Service.Domain.Models.Filters;
using Wastelands.Service.Domain.Models.Requests;

namespace Wastelands.Service.Infrastructure.Services
{
	public class GameService : IGameService
	{
		private readonly IGameRepository _gameRepository;
		private readonly IUserGameRepository _userGameRepository;
		private readonly IGameJoinRequestRepository _gameJoinRequestRepository;
		private readonly IMapper _mapper;
		private readonly IUserContext _userContext;

		public GameService(
			IGameRepository gameRepository,
			IUserGameRepository userGameRepository,
			IGameJoinRequestRepository gameJoinRequestRepository,
			IMapper mapper,
			IUserContext userContext)
		{
			_gameRepository = gameRepository;
			_userGameRepository = userGameRepository;
			_gameJoinRequestRepository = gameJoinRequestRepository;
			_mapper = mapper;
			_userContext = userContext;
		}

		public async Task<GameDto> GetGameByIdAsync(long id)
		{
			var game = await GetByIdAsync(id);
			var dto = _mapper.Map<GameDto>(game);
			dto.MembershipStatus = await GetMembershipStatusAsync(game);

			return dto;
		}

		public async Task<List<GameDto>> GetGamesAsync(GameFilter filter)
		{
			var games = await _gameRepository.GetListByFilterAsync(filter);
			var dtos = _mapper.Map<List<GameDto>>(games);

			var currentUserId = _userContext.CurrentUserId;

			var myMemberships = await _userGameRepository.GetListByFilterAsync(new UserGameFilter { UserId = currentUserId });
			var myGameIds = myMemberships.Select(x => x.GameId).ToHashSet();

			var myRequests = await _gameJoinRequestRepository.GetListByFilterAsync(
				new GameJoinRequestFilter { UserId = currentUserId });
			var myPendingGameIds = myRequests.Where(x => x.Status == GameJoinRequestStatus.Pending).Select(x => x.GameId).ToHashSet();
			var myDeclinedGameIds = myRequests.Where(x => x.Status == GameJoinRequestStatus.Declined).Select(x => x.GameId).ToHashSet();

			foreach (var dto in dtos)
			{
				dto.MembershipStatus = dto.CreatedByUserId == currentUserId
					? GameMembershipStatus.Owner
					: myGameIds.Contains(dto.Id)
						? GameMembershipStatus.Member
						: myPendingGameIds.Contains(dto.Id)
							? GameMembershipStatus.RequestPending
							: myDeclinedGameIds.Contains(dto.Id)
								? GameMembershipStatus.Declined
								: GameMembershipStatus.None;
			}

			return dtos;
		}

		public async Task<List<GameDto>> GetMyGamesAsync()
		{
			var currentUserId = _userContext.CurrentUserId;
			var games = await _gameRepository.GetMyGamesAsync(currentUserId);
			var dtos = _mapper.Map<List<GameDto>>(games);

			foreach (var dto in dtos)
			{
				dto.MembershipStatus = dto.CreatedByUserId == currentUserId ? GameMembershipStatus.Owner : GameMembershipStatus.Member;
			}

			return dtos;
		}

		public async Task<GameDto> CreateGameAsync(CreateGameRequest request)
		{
			var entity = new Game(request.Name, _userContext.CurrentUserId);

			await _gameRepository.CreateAsync(entity);

			var dto = _mapper.Map<GameDto>(entity);
			dto.MembershipStatus = GameMembershipStatus.Owner;

			return dto;
		}

		public async Task<GameDto> UpdateGameAsync(UpdateGameRequest request)
		{
			var game = await GetByIdAsync(request.Id);
			ThrowIfNotOwner(game);

			game.UpdateGame(request.Name);
			await _gameRepository.UpdateAsync(game);

			var dto = _mapper.Map<GameDto>(game);
			dto.MembershipStatus = GameMembershipStatus.Owner;

			return dto;
		}

		public async Task DeleteGameAsync(long id)
		{
			var game = await GetByIdAsync(id);
			ThrowIfNotOwner(game);

			await _gameRepository.DeleteAsync(game);
		}

		private async Task<Game> GetByIdAsync(long id)
		{
			var game = await _gameRepository.GetByIdAsync(id);

			NotFoundException.ThrowIfNull(
				game,
				ErrorCode.GameNotFound,
				nameof(Game),
				nameof(Game.Id),
				id.ToString());

			return game;
		}

		private void ThrowIfNotOwner(Game game)
		{
			if (game.CreatedByUserId != _userContext.CurrentUserId)
			{
				throw new InvalidArgumentException(
					ErrorCode.CurrentUserNotAllowedToPerformThisAction,
					"Только создатель игры может выполнить это действие.");
			}
		}

		private async Task<GameMembershipStatus> GetMembershipStatusAsync(Game game)
		{
			var currentUserId = _userContext.CurrentUserId;

			if (game.CreatedByUserId == currentUserId)
				return GameMembershipStatus.Owner;

			if (await _userGameRepository.AnyAsync(x => x.GameId == game.Id && x.UserId == currentUserId))
				return GameMembershipStatus.Member;

			if (await _gameJoinRequestRepository.AnyAsync(x => x.GameId == game.Id && x.UserId == currentUserId && x.Status == GameJoinRequestStatus.Pending))
				return GameMembershipStatus.RequestPending;

			if (await _gameJoinRequestRepository.AnyAsync(x => x.GameId == game.Id && x.UserId == currentUserId && x.Status == GameJoinRequestStatus.Declined))
				return GameMembershipStatus.Declined;

			return GameMembershipStatus.None;
		}
	}
}
