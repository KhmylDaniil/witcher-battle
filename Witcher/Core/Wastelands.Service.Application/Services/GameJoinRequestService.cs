using AutoMapper;
using Wastelands.Core.Contracts.Contracts;
using Wastelands.Core.Contracts.Enums;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Service.Application.Contracts;
using Wastelands.Service.Application.Contracts.Repositories;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;
using Wastelands.Service.Application.Models.Dto;
using Wastelands.Service.Application.Models.Filters;

namespace Wastelands.Service.Application.Services
{
	public class GameJoinRequestService : IGameJoinRequestService
	{
		private readonly IGameJoinRequestRepository _gameJoinRequestRepository;
		private readonly IGameRepository _gameRepository;
		private readonly IUserGameRepository _userGameRepository;
		private readonly IGameAccessGuard _gameAccessGuard;
		private readonly IMapper _mapper;
		private readonly IUserContext _userContext;

		public GameJoinRequestService(
			IGameJoinRequestRepository gameJoinRequestRepository,
			IGameRepository gameRepository,
			IUserGameRepository userGameRepository,
			IGameAccessGuard gameAccessGuard,
			IMapper mapper,
			IUserContext userContext)
		{
			_gameJoinRequestRepository = gameJoinRequestRepository;
			_gameRepository = gameRepository;
			_userGameRepository = userGameRepository;
			_gameAccessGuard = gameAccessGuard;
			_mapper = mapper;
			_userContext = userContext;
		}

		public async Task<GameJoinRequestDto> CreateJoinRequestAsync(long gameId)
		{
			var game = await GetGameByIdAsync(gameId);
			var currentUserId = _userContext.CurrentUserId;

			if (game.CreatedByUserId == currentUserId)
				throw new InvalidArgumentException(ErrorCode.UserAlreadyGameMember, "Создатель игры не может отправить заявку на присоединение к своей же игре.");

			if (await _userGameRepository.AnyAsync(x => x.GameId == gameId && x.UserId == currentUserId))
				throw new InvalidArgumentException(ErrorCode.UserAlreadyGameMember, "Вы уже участвуете в этой игре.");

			if (await _gameJoinRequestRepository.AnyAsync(x => x.GameId == gameId && x.UserId == currentUserId && x.Status == GameJoinRequestStatus.Pending))
				throw new InvalidArgumentException(ErrorCode.GameJoinRequestAlreadyPending, "Заявка на присоединение к этой игре уже отправлена.");

			var entity = new GameJoinRequest(currentUserId, gameId);
			await _gameJoinRequestRepository.CreateAsync(entity);

			return _mapper.Map<GameJoinRequestDto>(entity);
		}

		public async Task<List<GameJoinRequestDto>> GetMyJoinRequestsAsync()
		{
			var requests = await _gameJoinRequestRepository.GetListByFilterAsync(
				new GameJoinRequestFilter { UserId = _userContext.CurrentUserId });

			return _mapper.Map<List<GameJoinRequestDto>>(requests);
		}

		public async Task<List<GameJoinRequestDto>> GetIncomingJoinRequestsAsync(long gameId)
		{
			var game = await GetGameByIdAsync(gameId);
			_gameAccessGuard.EnsureOwner(game);

			var requests = await _gameJoinRequestRepository.GetListByFilterAsync(
				new GameJoinRequestFilter { GameId = gameId, Status = GameJoinRequestStatus.Pending });

			return _mapper.Map<List<GameJoinRequestDto>>(requests);
		}

		public async Task AcceptJoinRequestAsync(long requestId)
		{
			var request = await GetRequestByIdAsync(requestId);
			var game = await GetGameByIdAsync(request.GameId);
			_gameAccessGuard.EnsureOwner(game);

			request.Accept();
			await _gameJoinRequestRepository.UpdateAsync(request);

			await _userGameRepository.CreateAsync(new UserGame(request.UserId, request.GameId));
		}

		public async Task DeclineJoinRequestAsync(long requestId)
		{
			var request = await GetRequestByIdAsync(requestId);
			var game = await GetGameByIdAsync(request.GameId);
			_gameAccessGuard.EnsureOwner(game);

			request.Decline();
			await _gameJoinRequestRepository.UpdateAsync(request);
		}

		private async Task<GameJoinRequest> GetRequestByIdAsync(long id)
		{
			var request = await _gameJoinRequestRepository.GetByIdAsync(id);

			NotFoundException.ThrowIfNull(
				request,
				ErrorCode.GameJoinRequestNotFound,
				nameof(GameJoinRequest),
				nameof(GameJoinRequest.Id),
				id.ToString());

			return request;
		}

		private async Task<Game> GetGameByIdAsync(long id)
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

	}
}
