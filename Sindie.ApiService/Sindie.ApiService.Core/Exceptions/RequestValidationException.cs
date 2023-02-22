using System;

namespace Sindie.ApiService.Core.Exceptions
{
	/// <summary>
	/// Исключение валидации запроса
	/// </summary>
	public class RequestValidationException : Exception
	{
		/// <summary>
		/// Сообщение для вывода пользователю
		/// </summary>
		public string UserMessage { get; set; }

		/// <summary>
		/// Конструктор исключения приложения
		/// </summary>
		/// <param name="message">Текст исключения</param>
		public RequestValidationException(string message) : base(message)
		{
			UserMessage = message;
		}
	}
}
