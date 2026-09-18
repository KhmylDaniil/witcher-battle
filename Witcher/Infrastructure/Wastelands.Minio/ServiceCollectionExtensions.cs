using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using global::Minio;
using Wastelands.Service.Application.Contracts;

namespace Wastelands.Minio
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddMinioImageStorage(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<MinioOptions>(configuration.GetSection(MinioOptions.SectionName));

			var minioOptions = configuration.GetSection(MinioOptions.SectionName).Get<MinioOptions>();
			services.AddSingleton<IMinioClient>(_ => new MinioClient()
				.WithEndpoint(minioOptions.Endpoint)
				.WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey)
				.WithSSL(minioOptions.UseSSL)
				.Build());
			services.AddSingleton<IImageStorage, MinioImageStorage>();

			return services;
		}
	}
}
