namespace Wastelands.Service.Application.Options
{
	/// <summary>
	/// Параметры подключения к MinIO — хранилищу картинок (аватарки существ/персонажей).
	/// </summary>
	public class MinioOptions
	{
		public const string SectionName = "Minio";

		public string Endpoint { get; set; }

		public string AccessKey { get; set; }

		public string SecretKey { get; set; }

		public string Bucket { get; set; }

		public bool UseSSL { get; set; }
	}
}
