using AutoMapper;
using Wastelands.Core.Contracts.Enums;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Service.Application.Contracts;
using Wastelands.Service.Application.Contracts.Repositories;
using Wastelands.Service.Application.Models.Dto;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Application.Services
{
	/// <summary>Загрузка/удаление изображения шаблона существа — см. ICreatureTemplateImageService.</summary>
	public class CreatureTemplateImageService : ICreatureTemplateImageService
	{
		private readonly ICreatureTemplateRepository _creatureTemplateRepository;
		private readonly IImageStorage _imageStorage;
		private readonly IMapper _mapper;

		public CreatureTemplateImageService(ICreatureTemplateRepository creatureTemplateRepository, IImageStorage imageStorage, IMapper mapper)
		{
			_creatureTemplateRepository = creatureTemplateRepository;
			_imageStorage = imageStorage;
			_mapper = mapper;
		}

		public async Task<CreatureTemplateDto> SetImageAsync(long creatureTemplateId, Stream content, string contentType, long contentLength)
		{
			ImageValidation.EnsureValid(contentType, contentLength);

			var creatureTemplate = await GetByIdAsync(creatureTemplateId);
			var previousImageKey = creatureTemplate.ImageKey;

			var newImageKey = await _imageStorage.UploadAsync(content, contentType);
			creatureTemplate.SetImage(newImageKey);
			await _creatureTemplateRepository.UpdateAsync(creatureTemplate);

			if (previousImageKey is not null)
			{
				await _imageStorage.DeleteAsync(previousImageKey);
			}

			return _mapper.Map<CreatureTemplateDto>(creatureTemplate);
		}

		public async Task<CreatureTemplateDto> RemoveImageAsync(long creatureTemplateId)
		{
			var creatureTemplate = await GetByIdAsync(creatureTemplateId);
			var previousImageKey = creatureTemplate.ImageKey;

			creatureTemplate.SetImage(null);
			await _creatureTemplateRepository.UpdateAsync(creatureTemplate);

			if (previousImageKey is not null)
			{
				await _imageStorage.DeleteAsync(previousImageKey);
			}

			return _mapper.Map<CreatureTemplateDto>(creatureTemplate);
		}

		private async Task<CreatureTemplate> GetByIdAsync(long id)
		{
			var creatureTemplate = await _creatureTemplateRepository.GetByIdAsync(id);

			NotFoundException.ThrowIfNull(
				creatureTemplate, ErrorCode.CreatureTemplateNotFound, nameof(CreatureTemplate), nameof(CreatureTemplate.Id), id.ToString());

			return creatureTemplate;
		}
	}
}
