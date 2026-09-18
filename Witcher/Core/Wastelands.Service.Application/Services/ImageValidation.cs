using Wastelands.Core.Contracts.Enums;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;

namespace Wastelands.Service.Application.Services
{
	/// <summary>
	/// Общая валидация загружаемых файлов изображений — переиспользуется CreatureTemplateImageService
	/// и CharacterImageService вместо двух копий одной и той же проверки.
	/// </summary>
	internal static class ImageValidation
	{
		private const long MaxContentLength = 5 * 1024 * 1024;

		private static readonly HashSet<string> AllowedContentTypes =
			new(StringComparer.OrdinalIgnoreCase) { "image/png", "image/jpeg", "image/webp", "image/gif" };

		public static void EnsureValid(string contentType, long contentLength)
		{
			if (!AllowedContentTypes.Contains(contentType))
			{
				throw new InvalidArgumentException(ErrorCode.UnsupportedImageType, "Поддерживаются только изображения PNG, JPEG, WEBP или GIF.");
			}

			if (contentLength > MaxContentLength)
			{
				throw new InvalidArgumentException(ErrorCode.ImageTooLarge, "Размер изображения не должен превышать 5 МБ.");
			}
		}
	}
}
