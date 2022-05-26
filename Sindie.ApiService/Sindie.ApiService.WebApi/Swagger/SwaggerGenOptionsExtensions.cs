using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Reflection;

namespace Sindie.ApiService.WebApi.Swagger
{
	/// <summary>
	/// Расширения для <see cref="SwaggerGenOptions"/>
	/// </summary>
	public static class SwaggerGenOptionsExtensions
	{
		/// <summary>
		/// Добавить комментарии xmldoc к документации сваггера
		/// </summary>
		/// <param name="options">Опции сваггера</param>
		/// <param name="assembly">Сборка с комментами</param>
		/// <returns>Опции сваггера с комментами</returns>
		public static SwaggerGenOptions IncludeXmlCommentsIfExists(this SwaggerGenOptions options, Assembly assembly)
		{
			if (options is null)
				throw new ArgumentNullException(nameof(options));

			if (assembly is null)
				throw new ArgumentNullException(nameof(assembly));

			if (string.IsNullOrEmpty(assembly.Location))
				return options;

			var filePath = Path.ChangeExtension(assembly.Location, ".xml");
			IncludeXmlCommentsIfExists(options, filePath);
			return options;
		}

		/// <summary>
		/// Добавить в сваггер комменты из файла с xmldoc
		/// </summary>
		/// <param name="options">Опции сваггера</param>
		/// <param name="filePath">Путь к файлу с xmldoc</param>
		/// <returns><c>true</c> если удалось найти и добавить комменты к сваггеру <c>false</c>.</returns>
		public static bool IncludeXmlCommentsIfExists(this SwaggerGenOptions options, string filePath)
		{
			if (options is null)
				throw new ArgumentNullException(nameof(options));

			if (filePath is null)
				throw new ArgumentNullException(nameof(filePath));

			if (!File.Exists(filePath))
				return false;

			options.IncludeXmlComments(filePath);
			return true;
		}
	}
}
