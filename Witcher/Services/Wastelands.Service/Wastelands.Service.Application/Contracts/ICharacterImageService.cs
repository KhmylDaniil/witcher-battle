using Wastelands.Service.Application.Models.Dto;

namespace Wastelands.Service.Application.Contracts
{
	/// <summary>
	/// Загрузка/удаление изображения персонажа — отдельно от ICharacterService, т.к. это единственное
	/// место, которому нужна зависимость IImageStorage.
	/// </summary>
	public interface ICharacterImageService
	{
		Task<CharacterDto> SetImageAsync(long characterId, Stream content, string contentType, long contentLength);

		Task<CharacterDto> RemoveImageAsync(long characterId);
	}
}
