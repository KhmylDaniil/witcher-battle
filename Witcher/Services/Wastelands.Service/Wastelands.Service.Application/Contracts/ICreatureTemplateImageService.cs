using Wastelands.Service.Application.Models.Dto;

namespace Wastelands.Service.Application.Contracts
{
	/// <summary>
	/// Загрузка/удаление изображения шаблона существа — отдельно от ICreatureTemplateService
	/// (статблок/части тела/навыки/модификаторы урона), т.к. это единственное место, которому нужна
	/// зависимость IImageStorage.
	/// </summary>
	public interface ICreatureTemplateImageService
	{
		Task<CreatureTemplateDto> SetImageAsync(long creatureTemplateId, Stream content, string contentType, long contentLength);

		Task<CreatureTemplateDto> RemoveImageAsync(long creatureTemplateId);
	}
}
