using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;

namespace Sindie.ApiService.WebApi.Swagger
{
	/// <summary>
	/// Фильтр для явной документации конкретной версии API
	/// </summary>
	public class ApiVersionOperationFilter : IOperationFilter
	{
		/// <inheritdoc/>
		public void Apply(OpenApiOperation operation, OperationFilterContext context)
		{
			if (operation is null)
				throw new ArgumentNullException(nameof(operation));

			if (context is null)
				throw new ArgumentNullException(nameof(context));

			var apiDescription = context.ApiDescription;
			operation.Deprecated |= apiDescription.IsDeprecated();

			if (operation.Parameters is null)
				return;

			foreach (var parameter in operation.Parameters)
			{
				var description = apiDescription.ParameterDescriptions
					.First(x => string.Equals(x.Name, parameter.Name, StringComparison.OrdinalIgnoreCase));

				if (parameter.Description is null)
					parameter.Description = description.ModelMetadata?.Description;

				if (parameter.Schema.Default is null && description.DefaultValue != null)
					parameter.Schema.Default = new OpenApiString(description.DefaultValue.ToString());

				parameter.Required |= description.IsRequired;
			}
		}
	}
}
