using AutoMapper;
using Wastelands.Core.Contracts.Enums;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Service.Application.Contracts;
using Wastelands.Service.Application.Contracts.Repositories;
using Wastelands.Service.Application.Models.Dto;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Application.Services
{
	/// <summary>Загрузка/удаление изображения персонажа — см. ICharacterImageService.</summary>
	public class CharacterImageService : ICharacterImageService
	{
		private readonly ICharacterRepository _characterRepository;
		private readonly IImageStorage _imageStorage;
		private readonly IMapper _mapper;

		public CharacterImageService(ICharacterRepository characterRepository, IImageStorage imageStorage, IMapper mapper)
		{
			_characterRepository = characterRepository;
			_imageStorage = imageStorage;
			_mapper = mapper;
		}

		public async Task<CharacterDto> SetImageAsync(long characterId, Stream content, string contentType, long contentLength)
		{
			ImageValidation.EnsureValid(contentType, contentLength);

			var character = await GetByIdAsync(characterId);
			var previousImageKey = character.ImageKey;

			var newImageKey = await _imageStorage.UploadAsync(content, contentType);
			character.SetImage(newImageKey);
			await _characterRepository.UpdateAsync(character);

			if (previousImageKey is not null)
			{
				await _imageStorage.DeleteAsync(previousImageKey);
			}

			return _mapper.Map<CharacterDto>(character);
		}

		public async Task<CharacterDto> RemoveImageAsync(long characterId)
		{
			var character = await GetByIdAsync(characterId);
			var previousImageKey = character.ImageKey;

			character.SetImage(null);
			await _characterRepository.UpdateAsync(character);

			if (previousImageKey is not null)
			{
				await _imageStorage.DeleteAsync(previousImageKey);
			}

			return _mapper.Map<CharacterDto>(character);
		}

		private async Task<Character> GetByIdAsync(long id)
		{
			var character = await _characterRepository.GetByIdAsync(id);

			NotFoundException.ThrowIfNull(character, ErrorCode.CharacterNotFound, nameof(Character), nameof(Character.Id), id.ToString());

			return character;
		}
	}
}
