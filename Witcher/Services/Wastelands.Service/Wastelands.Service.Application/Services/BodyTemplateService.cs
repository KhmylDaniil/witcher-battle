using AutoMapper;
using Wastelands.Core.Contracts.Enums;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Service.Application.Contracts;
using Wastelands.Service.Application.Contracts.Repositories;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Application.Models.Dto;
using Wastelands.Service.Application.Models.Filters;
using Wastelands.Service.Application.Models.Requests;

namespace Wastelands.Service.Application.Services
{
	public class BodyTemplateService : IBodyTemplateService
	{
		private readonly IBodyTemplateRepository _bodyTemplateRepository;
		private readonly IGameAccessGuard _gameAccessGuard;
		private readonly IMapper _mapper;

		public BodyTemplateService(
			IBodyTemplateRepository bodyTemplateRepository,
			IGameAccessGuard gameAccessGuard,
			IMapper mapper)
		{
			_bodyTemplateRepository = bodyTemplateRepository;
			_gameAccessGuard = gameAccessGuard;
			_mapper = mapper;
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
			var game = await _gameAccessGuard.GetGameOwnedByCurrentUserAsync(request.GameId);

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
	}
}
