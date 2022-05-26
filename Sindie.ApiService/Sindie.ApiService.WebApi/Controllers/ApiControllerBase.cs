using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Sindie.ApiService.WebApi.Controllers
{
	/// <summary>
	/// Базовый API-контроллер
	/// </summary>
	[Route("api/v{version:apiVersion}/[controller]")]
	[ApiController]
	[SwaggerResponse(StatusCodes.Status500InternalServerError, type: typeof(ProblemDetails))]
	public class ApiControllerBase : ControllerBase
	{
	}
}
