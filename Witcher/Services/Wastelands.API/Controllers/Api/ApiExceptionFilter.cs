using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Wastelands.Core.Contracts.Exceptions;

namespace Wastelands.API.Controllers.Api
{
	/// <summary>
	/// Единая обработка исключений для JSON API — аналог ExceptionFilter (Filters/ExceptionFilter.cs), но вместо
	/// пересборки Razor View возвращает JSON. Каждое исключение в Wastelands уже несёт свой StatusCode
	/// (BaseException.StatusCode, переопределён в NotFoundException/BadRequestException/UnauthorizedException) —
	/// поэтому, в отличие от Witcher, маппинг тривиален: одно свойство, а не switch по типам.
	/// </summary>
	public class ApiExceptionFilter : IExceptionFilter
	{
		private readonly ILogger<ApiExceptionFilter> _logger;

		public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
		{
			_logger = logger;
		}

		public void OnException(ExceptionContext context)
		{
			var ex = context.Exception;

			int statusCode;
			object body;

			if (ex is BaseException baseException)
			{
				statusCode = baseException.StatusCode;
				body = new { errorCode = baseException.ErrorCode, message = baseException.Message };
			}
			else
			{
				statusCode = StatusCodes.Status500InternalServerError;
				_logger.LogError(ex, "Unhandled exception in {Action}", context.ActionDescriptor.DisplayName);
				body = new { message = "Внутренняя ошибка сервера." };
			}

			context.Result = new ObjectResult(body) { StatusCode = statusCode };
			context.ExceptionHandled = true;
		}
	}
}
