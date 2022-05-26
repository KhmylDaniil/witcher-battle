using System;

namespace Sindie.ApiService.Core.Exceptions
{
	/// <summary>
	/// Базовое исключение приложения (код 400 bad request)
	/// </summary>
	public class ExceptionApplicationBase : Exception
	{
		/// <summary>
		/// Конструктор исключения приложения
		/// </summary>
		/// <param name="message">Текст исключения</param>
		public ExceptionApplicationBase(string message) : base(message)
		{
		}
	}
}
