using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.Exceptions.ScryptExceptions;
using Sindie.ApiService.Core.Exceptions.UnauthorizedExceptions;
using System;
using System.Threading.Tasks;

namespace Sindie.ApiService.WebApi.ExseptionMiddelware
{
	/// <summary>
	/// Обработка исключений Мидделвейр
	/// </summary>
	public class ExсeptionHandlingMiddelware
	{
		/// <summary>
		/// Следующий
		/// </summary>
		private readonly RequestDelegate _next;

		/// <summary>
		/// Логгер
		/// </summary>
		private readonly ILogger<ExсeptionHandlingMiddelware> logger;

		/// <summary>
		/// Конструктор обработчика исключений Мидделвейр
		/// </summary>
		/// <param name="next">Следующий</param>
		/// <param name="logger">Логгер</param>
		public ExсeptionHandlingMiddelware(RequestDelegate next, ILogger<ExсeptionHandlingMiddelware> logger)
		{
			_next = next;
			this.logger = logger;
		}

		/// <summary>
		/// Асинхронный вызов
		/// </summary>
		/// <param name="context">Контекст</param>
		/// <returns>Следующий Мидделвейр</returns>
		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next.Invoke(context);
			}
			catch (ExceptionNotFoundBase ex)
			{
				await ProcessException(context, ex, 404);
			}
			catch (ExceptionUnauthorizedBase ex)
			{
				await ProcessException(context, ex, 403);
			}
			catch (ExceptionScryptBase ex)
			{
				await ProcessException(context, ex, 420);
			}
			catch (ExceptionApplicationBase ex)
			{
				await ProcessException(context, ex, 400);
			}
			catch (Exception ex)
			{
				await ProcessException(context, ex, 500);
			}
		}

		private async Task ProcessException(HttpContext context, Exception ex, int statusCode)
		{
			logger.LogError(ex.ToString());
			context.Response.Clear();
			var Response = JsonConvert.SerializeObject(new Error { Text = ex.Message });
			context.Response.StatusCode = statusCode;
			context.Response.ContentType = "application/json";
			await context.Response.WriteAsync(Response);
		}
	}
}
