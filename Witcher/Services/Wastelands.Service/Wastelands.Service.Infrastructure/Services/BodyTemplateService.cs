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
	public class BodyTemplateService : IBodyTemplateService
	{
		private readonly IBodyTemplateRepository _bodyTemplateRepository;
		private readonly IGameRepository _gameRepository;
		private readonly IMapper _mapper;
		private readonly IUserContext _userContext;

		public BodyTemplateService(
			IBodyTemplateRepository bodyTemplateRepository,
			IGameRepository gameRepository,
			IMapper mapper,
			IUserContext userContext)
		{
			_bodyTemplateRepository = bodyTemplateRepository;
			_gameRepository = gameRepository;
			_mapper = mapper;
			_userContext = userContext;
		}

		public async Task<BodyTemplateDto> GetBodyTemplateByIdAsync(long id)
		{
			var bodyTemplate = await GetByIdAsync(id);
			return _mapper.Map<BodyTemplateDto>(bodyTemplate);
		}

		public async Task<List<BodyTemplateDto>> GetBodyTemplatesAsync(BodyTemplateFilter filter)
		{
			var bodyTemplates = await _bodyTemplateRepository.GetListByFilterAsync(filter);
			return _mapper.Map<List<BodyTemplateDto>>(bodyTemplates);
		}

		public async Task<BodyTemplateDto> CreateBodyTemplateAsync(CreateBodyTemplateRequest request)
		{
			var game = await GetGameOwnedByCurrentUserAsync(request.GameId);

			var entity = new BodyTemplate(game.Id, request.Name, request.Description);
			await _bodyTemplateRepository.CreateAsync(entity);

			return _mapper.Map<BodyTemplateDto>(entity);
		}

		public async Task<BodyTemplateDto> UpdateBodyTemplateAsync(UpdateBodyTemplateRequest request)
		{
			var bodyTemplate = await GetByIdAsync(request.Id);
			bodyTemplate.UpdateBodyTemplate(request.Name, request.Description);

			await _bodyTemplateRepository.UpdateAsync(bodyTemplate);

			return _mapper.Map<BodyTemplateDto>(bodyTemplate);
		}

		public async Task DeleteBodyTemplateAsync(long id)
		{
			var bodyTemplate = await GetByIdAsync(id);
			await _bodyTemplateRepository.DeleteAsync(bodyTemplate);
		}

		public async Task<BodyTemplateDto> AddPartAsync(CreateBodyTemplatePartRequest request)
		{
			var bodyTemplate = await GetByIdAsync(request.BodyTemplateId);
			bodyTemplate.AddPart(request.Name, request.BodyPartType, request.DamageModifier, request.HitPenalty, request.MinToHit, request.MaxToHit);

			await _bodyTemplateRepository.UpdateAsync(bodyTemplate);

			return _mapper.Map<BodyTemplateDto>(bodyTemplate);
		}

		public async Task<BodyTemplateDto> UpdatePartAsync(UpdateBodyTemplatePartRequest request)
		{
			var bodyTemplate = await GetByIdAsync(request.BodyTemplateId);
			bodyTemplate.UpdatePart(request.PartId, request.Name, request.BodyPartType, request.DamageModifier, request.HitPenalty, request.MinToHit, request.MaxToHit);

			await _bodyTemplateRepository.UpdateAsync(bodyTemplate);

			return _mapper.Map<BodyTemplateDto>(bodyTemplate);
		}

		public async Task<BodyTemplateDto> RemovePartAsync(long bodyTemplateId, long partId)
		{
			var bodyTemplate = await GetByIdAsync(bodyTemplateId);
			bodyTemplate.RemovePart(partId);

			await _bodyTemplateRepository.UpdateAsync(bodyTemplate);

			return _mapper.Map<BodyTemplateDto>(bodyTemplate);
		}

		private async Task<BodyTemplate> GetByIdAsync(long id)
		{
			var bodyTemplate = await _bodyTemplateRepository.GetByIdAsync(id);

			NotFoundException.ThrowIfNull(
				bodyTemplate,
				ErrorCode.BodyTemplateNotFound,
				nameof(BodyTemplate),
				nameof(BodyTemplate.Id),
				id.ToString());

			return bodyTemplate;
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
	}
}
