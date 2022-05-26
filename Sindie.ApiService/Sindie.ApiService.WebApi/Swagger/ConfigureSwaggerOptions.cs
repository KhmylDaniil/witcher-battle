using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Sindie.ApiService.WebApi.Swagger
{
	/// <summary>
	/// Конфиг сваггера
	/// </summary>
	public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
	{
		private readonly IApiVersionDescriptionProvider _provider;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="provider">Провайдер версий API</param>
		public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
			=> _provider = provider;

		/// <inheritdoc/>
		public void Configure(SwaggerGenOptions options)
		{
			options.DescribeAllParametersInCamelCase();
			options.EnableAnnotations();

			options.IncludeXmlCommentsIfExists(typeof(Startup).Assembly);
			options.IncludeXmlCommentsIfExists(typeof(Core.Entry).Assembly);
			options.IncludeXmlCommentsIfExists(typeof(Storage.Postgresql.Entry).Assembly);

			options.OperationFilter<ApiVersionOperationFilter>();
			options.OperationFilter<RemoveVersionFromParameter>();
			options.DocumentFilter<ReplaceVersionWithExactValueInPath>();

			foreach (var apiVersionDescription in _provider.ApiVersionDescriptions)
			{
				var info = new OpenApiInfo()
				{
					Title = AssemblyInformation.Current.Product,
					Description = apiVersionDescription.IsDeprecated ?
						$"{AssemblyInformation.Current.Description} This API version has been deprecated." :
						AssemblyInformation.Current.Description,
					Version = apiVersionDescription.ApiVersion.ToString(),
				};
				options.SwaggerDoc(apiVersionDescription.GroupName, info);
			}
		}
	}
}
