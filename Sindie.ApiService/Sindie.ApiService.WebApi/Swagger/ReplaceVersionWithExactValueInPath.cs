using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Sindie.ApiService.WebApi.Swagger
{
	/// <summary>
	/// Подставить версию API для каждого определения версии сваггера
	/// </summary>
	public class ReplaceVersionWithExactValueInPath : IDocumentFilter
	{
		/// <inheritdoc/>
		public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
		{
			var paths = new OpenApiPaths();
			foreach (var (key, value) in swaggerDoc.Paths)
				paths.Add(key.Replace("{version}", swaggerDoc.Info.Version), value);
			swaggerDoc.Paths = paths;
		}
	}
}
