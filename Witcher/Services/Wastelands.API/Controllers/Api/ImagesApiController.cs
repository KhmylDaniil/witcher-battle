using Microsoft.AspNetCore.Mvc;
using Wastelands.Service.Application.Contracts;

namespace Wastelands.API.Controllers.Api
{
	/// <summary>
	/// Отдача байт картинок из MinIO. Авторизация — только факт логина (ApiControllerBase),
	/// без проверки владения конкретным CreatureTemplate/Character: ключ — непредсказуемый GUID,
	/// никогда не перечисляется списком, а сам DTO, в котором он встречается (например,
	/// GET /battles/{id}/creatures/{creatureId}/sheet), уже прошёл нужную проверку доступа. См.
	/// обоснование в плане реализации MinIO.
	/// </summary>
	[Route("api/images")]
	public class ImagesApiController : ApiControllerBase
	{
		private readonly IImageStorage _imageStorage;

		public ImagesApiController(IImageStorage imageStorage)
		{
			_imageStorage = imageStorage;
		}

		[HttpGet("{key}")]
		public async Task<IActionResult> Get(string key)
		{
			var result = await _imageStorage.DownloadAsync(key);
			if (result is null)
			{
				return NotFound();
			}

			return File(result.Value.Content, result.Value.ContentType);
		}
	}
}
