using AutoMapper;
using Wastelands.Core.Contracts.Contracts;
using Wastelands.Core.Contracts.Enums;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Service.Domain.Contracts;
using Wastelands.Service.Domain.Contracts.Repositories;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Models.Dto;
using Wastelands.Service.Domain.Models.Filters;
using Wastelands.Service.Domain.Models.Requests;

namespace Wastelands.Service.Infrastructure.Services
{
	public class CreatureTemplateService : ICreatureTemplateService
	{
		private readonly ICreatureTemplateRepository _creatureTemplateRepository;
		private readonly IBodyTemplateRepository _bodyTemplateRepository;
		private readonly IGameRepository _gameRepository;
		private readonly IMapper _mapper;
		private readonly IUserContext _userContext;

		public CreatureTemplateService(
			ICreatureTemplateRepository creatureTemplateRepository,
			IBodyTemplateRepository bodyTemplateRepository,
			IGameRepository gameRepository,
			IMapper mapper,
			IUserContext userContext)
		{
			_creatureTemplateRepository = creatureTemplateRepository;
			_bodyTemplateRepository = bodyTemplateRepository;
			_gameRepository = gameRepository;
			_mapper = mapper;
			_userContext = userContext;
		}

		public async Task<CreatureTemplateDto> GetCreatureTemplateByIdAsync(long id)
		{
			var creatureTemplate = await GetByIdAsync(id);
			return _mapper.Map<CreatureTemplateDto>(creatureTemplate);
		}

		public async Task<List<CreatureTemplateDto>> GetCreatureTemplatesAsync(CreatureTemplateFilter filter)
		{
			var creatureTemplates = await _creatureTemplateRepository.GetListByFilterAsync(filter);
			return _mapper.Map<List<CreatureTemplateDto>>(creatureTemplates);
		}

		public async Task<CreatureTemplateDto> CreateCreatureTemplateAsync(CreateCreatureTemplateRequest request)
		{
			var game = await GetGameOwnedByCurrentUserAsync(request.GameId);
			var bodyTemplate = await GetOwnBodyTemplateForGameAsync(request.BodyTemplateId, game.Id);

			var entity = new CreatureTemplate(
				gameId: game.Id,
				bodyTemplate: bodyTemplate,
				creatureType: request.CreatureType,
				name: request.Name,
				description: request.Description,
				hp: request.HP,
				sta: request.Sta,
				@int: request.Int,
				@ref: request.Ref,
				dex: request.Dex,
				body: request.Body,
				emp: request.Emp,
				cra: request.Cra,
				will: request.Will,
				speed: request.Speed,
				luck: request.Luck);

			await _creatureTemplateRepository.CreateAsync(entity);

			return _mapper.Map<CreatureTemplateDto>(entity);
		}

		public async Task<CreatureTemplateDto> UpdateCreatureTemplateAsync(UpdateCreatureTemplateRequest request)
		{
			var creatureTemplate = await GetByIdAsync(request.Id);

			creatureTemplate.UpdateCreatureTemplate(
				creatureType: request.CreatureType,
				name: request.Name,
				description: request.Description,
				hp: request.HP,
				sta: request.Sta,
				@int: request.Int,
				@ref: request.Ref,
				dex: request.Dex,
				body: request.Body,
				emp: request.Emp,
				cra: request.Cra,
				will: request.Will,
				speed: request.Speed,
				luck: request.Luck);

			await _creatureTemplateRepository.UpdateAsync(creatureTemplate);

			return _mapper.Map<CreatureTemplateDto>(creatureTemplate);
		}

		public async Task<CreatureTemplateDto> UpdatePartArmorAsync(long creatureTemplateId, long partId, int armor)
		{
			var creatureTemplate = await GetByIdAsync(creatureTemplateId);
			var part = creatureTemplate.Parts.FirstOrDefault(x => x.Id == partId);

			NotFoundException.ThrowIfNull(
				part,
				ErrorCode.CreatureTemplatePartNotFound,
				nameof(CreatureTemplatePart),
				nameof(CreatureTemplatePart.Id),
				partId.ToString());

			part.UpdateArmor(armor);
			await _creatureTemplateRepository.UpdateAsync(creatureTemplate);

			return _mapper.Map<CreatureTemplateDto>(creatureTemplate);
		}

		public async Task DeleteCreatureTemplateAsync(long id)
		{
			var creatureTemplate = await GetByIdAsync(id);
			await _creatureTemplateRepository.DeleteAsync(creatureTemplate);
		}

		private async Task<CreatureTemplate> GetByIdAsync(long id)
		{
			var creatureTemplate = await _creatureTemplateRepository.GetByIdAsync(id);

			NotFoundException.ThrowIfNull(
				creatureTemplate,
				ErrorCode.CreatureTemplateNotFound,
				nameof(CreatureTemplate),
				nameof(CreatureTemplate.Id),
				id.ToString());

			return creatureTemplate;
		}

		private async Task<Game> GetGameOwnedByCurrentUserAsync(long gameId)
		{
			var game = await _gameRepository.GetByIdAsync(gameId);
			NotFoundException.ThrowIfNull(game, ErrorCode.GameNotFound, nameof(Game), nameof(Game.Id), gameId.ToString());

			if (game.CreatedByUserId != _userContext.CurrentUserId)
			{
				throw new InvalidArgumentException(
					ErrorCode.CurrentUserNotAllowedToPerformThisAction,
					"Шаблоны тела и существ может создавать только мастер игры.");
			}

			return game;
		}

		private async Task<BodyTemplate> GetOwnBodyTemplateForGameAsync(long bodyTemplateId, long gameId)
		{
			// BodyTemplateRepository уже скоупит выдачу на шаблоны игр текущего пользователя — если чужой
			// (или несуществующий) Id, здесь придёт null независимо от переданного gameId.
			var bodyTemplate = await _bodyTemplateRepository.GetByIdAsync(bodyTemplateId);
			NotFoundException.ThrowIfNull(bodyTemplate, ErrorCode.BodyTemplateNotFound, nameof(BodyTemplate), nameof(BodyTemplate.Id), bodyTemplateId.ToString());

			if (bodyTemplate.GameId != gameId)
			{
				throw new InvalidArgumentException(
					ErrorCode.BodyTemplateBelongsToAnotherGame,
					"Выбранный шаблон тела принадлежит другой игре.");
			}

			return bodyTemplate;
		}
	}
}
