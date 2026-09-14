using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using System;
using System.Linq;
using Witcher.Core.Exceptions;
using Witcher.Core.Exceptions.RequestExceptions;

namespace Witcher.MVC.Controllers.Api
{
	/// <summary>
	/// Единая обработка исключений для JSON API — аналог BaseController.HandleException/HandleExceptionAsync,
	/// но вместо View/Redirect возвращает JSON с корректным статус-кодом.
	/// </summary>
	public class ApiExceptionFilter : IExceptionFilter
	{
		public void OnException(ExceptionContext context)
		{
			var ex = context.Exception;

			(int statusCode, object body) = ex switch
			{
				ValidationException valEx => (
					StatusCodes.Status400BadRequest,
					(object)new
					{
						message = "Ошибка валидации.",
						errors = valEx.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage })
					}),

				RequestValidationException reqEx when IsNoAccessException(reqEx) => (
					StatusCodes.Status403Forbidden,
					new { message = reqEx.UserMessage }),

				RequestValidationException reqEx => (
					StatusCodes.Status400BadRequest,
					new { message = reqEx.UserMessage }),

				EntityBaseException entEx => (
					StatusCodes.Status404NotFound,
					new { message = entEx.Message }),

				_ => (StatusCodes.Status500InternalServerError, (object)null)
			};

			if (statusCode == StatusCodes.Status500InternalServerError)
			{
				Log.ForContext(context.ActionDescriptor.GetType()).Error(ex, ex.Message);
				body = new { message = "Внутренняя ошибка сервера." };
			}

			context.Result = new ObjectResult(body) { StatusCode = statusCode };
			context.ExceptionHandled = true;
		}

		/// <summary>
		/// NoAccessToEntityException&lt;T&gt; — открытый generic-тип, поэтому его нельзя сопоставить через обычный
		/// pattern matching по конкретному T; проверяем по generic type definition.
		/// </summary>
		private static bool IsNoAccessException(RequestValidationException ex)
		{
			var type = ex.GetType();
			return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(NoAccessToEntityException<>);
		}
	}
}
