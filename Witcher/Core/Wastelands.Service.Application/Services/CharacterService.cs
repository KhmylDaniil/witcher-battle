using AutoMapper;
using Wastelands.Core.Contracts.Constants;
using Wastelands.Core.Contracts.Contracts;
using Wastelands.Core.Contracts.Enums;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Core.Contracts.Exceptions.WebExceptions;
using Wastelands.Core.Contracts.Models;
using Wastelands.Service.Application.Contracts;
using Wastelands.Service.Application.Contracts.Repositories;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;
using Wastelands.Service.Application.Models.Dto;
using Wastelands.Service.Application.Models.Filters;
using Wastelands.Service.Application.Models.Requests;

namespace Wastelands.Service.Application.Services
{
	public class CharacterService : ICharacterService
	{
		private readonly IMapper _mapper;
		private readonly ICharacterRepository _characterRepository;
		private readonly IGameRepository _gameRepository;
		private readonly IUserGameRepository _userGameRepository;
		private readonly IBattleRepository _battleRepository;
		private readonly IUserContext _userContext;
		private readonly IImageStorage _imageStorage;

		public CharacterService(
			ICharacterRepository repository,
			IGameRepository gameRepository,
			IUserGameRepository userGameRepository,
			IBattleRepository battleRepository,
			IMapper mapper,
			IUserContext userContext,
			IImageStorage imageStorage)
		{
			_characterRepository = repository;
			_gameRepository = gameRepository;
			_userGameRepository = userGameRepository;
			_battleRepository = battleRepository;
			_mapper = mapper;
			_userContext = userContext;
			_imageStorage = imageStorage;
		}

		/// <summary>Чтение (в отличие от изменения) доступно ещё и мастеру игры персонажа — например, переход на лист персонажа со страницы боя.</summary>
		public async Task<CharacterDto> GetCharacterByIdAsync(long id)
		{
			var character = await _characterRepository.GetByIdAsync(id) ?? await GetIfCurrentUserIsGameMasterAsync(id);

			NotFoundException.ThrowIfNull(character, ErrorCode.CharacterNotFound, nameof(Character), nameof(Character.Id), id.ToString());

			return _mapper.Map<CharacterDto>(character);
		}

		private async Task<Character?> GetIfCurrentUserIsGameMasterAsync(long id)
		{
			var character = await _characterRepository.GetByIdUnscopedAsync(id);
			if (character?.GameId is not { } gameId)
			{
				return null;
			}

			var game = await _gameRepository.GetByIdAsync(gameId);
			return game?.CreatedByUserId == _userContext.CurrentUserId ? character : null;
		}

		public async Task<PagedResultDto<CharacterDto>> GetCharactersAsync(CharacterFilter filter, PagedRequest paging)
		{
			var paged = await _characterRepository.GetPagedAsync(paging, filter);
			var dtos = _mapper.Map<List<CharacterDto>>(paged.Entities);

			return new PagedResultDto<CharacterDto> { Items = dtos, TotalCount = paged.TotalCount, PageNumber = paging.PageNumber, PageSize = paging.PageSize };
		}

		public async Task<List<CharacterDto>> GetGameCharactersAsync(long gameId)
		{
			var game = await _gameRepository.GetByIdAsync(gameId);
			NotFoundException.ThrowIfNull(game, ErrorCode.GameNotFound, nameof(Game), nameof(Game.Id), gameId.ToString());

			if (game.CreatedByUserId != _userContext.CurrentUserId)
			{
				throw new InvalidArgumentException(
					ErrorCode.CurrentUserNotAllowedToPerformThisAction,
					"Персонажей игроков может смотреть только мастер игры.");
			}

			var characters = await _characterRepository.GetCharactersByGameIdAsync(gameId);
			return _mapper.Map<List<CharacterDto>>(characters);
		}

		public async Task<CharacterDto> CreateCharacterAsync(CreateCharacterRequest request)
		{
			var currentUserId = _userContext.CurrentUserId;

			var game = await _gameRepository.GetByIdAsync(request.GameId);
			NotFoundException.ThrowIfNull(game, ErrorCode.GameNotFound, nameof(Game), nameof(Game.Id), request.GameId.ToString());

			var isGameMember = game.CreatedByUserId == currentUserId
				|| await _userGameRepository.AnyAsync(x => x.GameId == request.GameId && x.UserId == currentUserId);

			if (!isGameMember)
			{
				throw new InvalidArgumentException(ErrorCode.UserNotGameMember, "Персонажа можно создать только в игре, в которой вы участвуете.");
			}

			if (await _characterRepository.AnyAsync(x => x.Name == request.Name))
			{
				throw new BadRequestException(ErrorCode.InvalidArgument, string.Format(ExceptionMessages.ValueMustBeUnique, nameof(request.Name)));
			}

			var entity = new Character(
				userId: currentUserId,
				gameId: request.GameId,
				name: request.Name,
				hp: request.HP,
				sta: request.Sta,
				@int: request.Int,
				str: request.Str,
				rea: request.Rea,
				dex: request.Dex,
				cra: request.Cra,
				emp: request.Emp,
				wil: request.Wil,
				movement: request.Movement);

			await _characterRepository.CreateAsync(entity);

			return _mapper.Map<CharacterDto>(entity);
		}

		public async Task<CharacterDto> RestAsync(long id)
		{
			var character = await GetByIdAsync(id);

			var inBattle = await _battleRepository.AnyAsync(
				b => b.Status == BattleStatus.InProgress && b.Characters.Any(bc => bc.CharacterId == id));
			if (inBattle)
			{
				throw new InvalidArgumentException(ErrorCode.CharacterCurrentlyInBattle, "Персонаж не может отдыхать, пока участвует в бою.");
			}

			character.Rest();
			await _characterRepository.UpdateAsync(character);

			return _mapper.Map<CharacterDto>(character);
		}

		public async Task<CharacterDto> UpdateCharacterAsync(UpdateCharacterRequest request)
		{
			if (await _characterRepository.AnyAsync(x => x.Name == request.Name && x.Id != request.Id))
			{
				throw new BadRequestException(ErrorCode.InvalidArgument, string.Format(ExceptionMessages.ValueMustBeUnique, nameof(request.Name)));
			}

			var character = await GetByIdAsync(request.Id);
			character.UpdateCharacter(
				name: request.Name,
				hp: request.HP,
				sta: request.Sta,
				@int: request.Int,
				str: request.Str,
				rea: request.Rea,
				dex: request.Dex,
				cra: request.Cra,
				emp: request.Emp,
				wil: request.Wil,
				movement: request.Movement);

			await _characterRepository.UpdateAsync(character);

			return _mapper.Map<CharacterDto>(character);
		}

		public async Task DeleteCharacterAsync(long id)
		{
			var character = await GetByIdAsync(id);
			await _characterRepository.DeleteAsync(character);

			if (character.ImageKey is not null)
			{
				await _imageStorage.DeleteAsync(character.ImageKey);
			}
		}

		public async Task AddSkillAsync(AddOrUpdateCharacterSkillRequest request)
		{
			var character = await GetByIdAsync(request.CharacterId);

			if (character.Skills.ContainsKey(request.Skill))
				throw new InvalidArgumentException(ErrorCode.CharacterSkillAlreadyExisted, string.Format(ExceptionMessages.ValueMustBeUnique, nameof(request.Skill)));

			await AddOrUpdateSkillAsync(character, request.Skill, request.Value);
		}

		public async Task UpdateSkillAsync(AddOrUpdateCharacterSkillRequest request)
		{
			var character = await GetByIdAsync(request.CharacterId);
			await AddOrUpdateSkillAsync(character, request.Skill, request.Value);
		}

		public async Task DeleteSkillAsync(DeleteCharacterSkillRequest request)
		{
			var character = await GetByIdAsync(request.Id);
			character.Skills.Remove(request.Skill);

			await _characterRepository.UpdateAsync(character);
		}

		private async Task<Character> GetByIdAsync(long id)
		{
			var character = await _characterRepository.GetByIdAsync(id);

			NotFoundException.ThrowIfNull(
			character,
			ErrorCode.CharacterNotFound,
			nameof(Character),
			nameof(Character.Id),
			id.ToString());

			return character;
		}

		private async Task AddOrUpdateSkillAsync(Character character, Skill skill, int value)
		{
			InvalidArgumentException.ThrowIfNotInRange(value, Character.MinSkillValue, Character.MaxSkillValue, nameof(value));

			character.Skills[skill] = value;
			await _characterRepository.UpdateAsync(character);
		}
	}
}
