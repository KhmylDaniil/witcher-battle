using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Sindie.ApiService.WebApi;
using Sindie.ApiService.WebApi.Swagger;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace Sindie.ApiService.WebApi.Swagger
{
	/// <summary>
	/// Точка входа сервисов swagger для приложения
	/// </summary>
	public static class Entry
	{
		/// <summary>
		/// Добавить в службы приложения swagger
		/// </summary>
		/// <param name="services">Службы приложения</param>
		/// <returns>Службы приложения со сваггером</returns>
		public static IServiceCollection AddCustomSwagger(this IServiceCollection services) =>
			services
				.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>()
				.AddSwaggerExamplesFromAssemblyOf(typeof(Entry))
				.AddSwaggerGen(c =>
				{
					c.ExampleFilters();

					c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
					{
						Name = "Authorization",
						In = ParameterLocation.Header,
						Type = SecuritySchemeType.ApiKey,
						Scheme = "Bearer",
					});

					c.AddSecurityRequirement(new OpenApiSecurityRequirement()
					{
						{
							new OpenApiSecurityScheme
							{
								Reference = new OpenApiReference
								{
									Type = ReferenceType.SecurityScheme,
									Id = "Bearer",
								},
								Scheme = "oauth2",
								Name = "Bearer",
								In = ParameterLocation.Header,
							},
							new List<string>()
						},
					});
				});

		/// <summary>
		/// Добавить кастомный UI сваггера
		/// </summary>
		/// <param name="application">Пайплайн приложения</param>
		/// <returns>Пайплайн приложения со сваггером</returns>
		public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder application) =>
			application
				.UseSwagger()
				.UseSwaggerUI(
					options =>
					{
						options.DocumentTitle = AssemblyInformation.Current.Product;
						options.RoutePrefix = "swagger";
						options.DisplayRequestDuration();

						var provider = application.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
						foreach (var apiVersionDescription in provider
							.ApiVersionDescriptions
							.OrderByDescending(x => x.ApiVersion))
						{
							options.SwaggerEndpoint(
								$"/swagger/{apiVersionDescription.GroupName}/swagger.json",
								$"Version {apiVersionDescription.ApiVersion}");
						}
					});
	}
}
