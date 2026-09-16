using AutoMapper;
using Wastelands.Core.Contracts.Constants;
using Wastelands.Core.Contracts.Contracts;
using Wastelands.Core.Contracts.Enums;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Core.Contracts.Exceptions.WebExceptions;
using Wastelands.Service.Domain.Contracts;
using Wastelands.Service.Domain.Contracts.Repositories;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;
using Wastelands.Service.Domain.Models.Dto;
using Wastelands.Service.Domain.Models.Filters;
using Wastelands.Service.Domain.Models.Requests;

namespace Wastelands.Service.Infrastructure.Services
{
	public class CharacterService : ICharacterService
	{
		private readonly IMapper _mapper;
		private readonly ICharacterRepository _characterRepository;
		private readonly IGameRepository _gameRepository;
		private readonly IUserGameRepository _userGameRepository;
		private readonly IUserContext _userContext;

		public CharacterService(
			ICharacterRepository repository,
			IGameRepository gameRepository,
			IUserGameRepository userGameRepository,
			IMapper mapper,
			IUserContext userContext)
		{
			_characterRepository = repository;
			_gameRepository = gameRepository;
			_userGameRepository = userGameRepository;
			_mapper = mapper;
			_userContext = userContext;
		}

		public async Task<CharacterDto> GetCharacterByIdAsync(long id)
		{
			var character = await GetByIdAsync(id);
			return _mapper.Map<CharacterDto>(character);
		}

		public async Task<List<CharacterDto>> GetCharactersAsync(CharacterFilter filter)
		{
			var characters = await _characterRepository.GetListByFilterAsync(filter);

			var dtos = _mapper.Map<List<CharacterDto>>(characters);

			return dtos;
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
				wil: request.Wil);

			await _characterRepository.CreateAsync(entity);

			return _mapper.Map<CharacterDto>(entity);
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
				wil: request.Wil);

			await _characterRepository.UpdateAsync(character);

			return _mapper.Map<CharacterDto>(character);
		}

		public async Task DeleteCharacterAsync(long id)
		{
			var character = await GetByIdAsync(id);
			await _characterRepository.DeleteAsync(character);
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
