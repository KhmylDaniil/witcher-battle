namespace Wastelands.Service.Application.Contracts
{
	/// <summary>
	/// Хранилище картинок (MinIO) — не привязано к конкретной доменной сущности. Ключи — GUID,
	/// никогда не перечисляются списком, поэтому знание ключа само по себе служит правом доступа
	/// (см. ImagesApiController).
	/// </summary>
	public interface IImageStorage
	{
		Task<string> UploadAsync(Stream content, string contentType, CancellationToken cancellationToken = default);

		Task<(Stream Content, string ContentType)?> DownloadAsync(string key, CancellationToken cancellationToken = default);

		Task DeleteAsync(string key, CancellationToken cancellationToken = default);

		Task EnsureBucketExistsAsync(CancellationToken cancellationToken = default);
	}
}
