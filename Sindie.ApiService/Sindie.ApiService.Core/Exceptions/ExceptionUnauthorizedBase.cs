using System;

namespace Witcher.Core.Exceptions
{
	/// <summary>
	/// Базовое исключение авторизации (код 403 Forbidden) 
	/// </summary>
	public class ExceptionUnauthorizedBase : Exception
	{
		/// <summary>
		/// Конструктор исключения авторизации
		/// </summary>
		/// <param name="message">Текст исключения</param>
		public ExceptionUnauthorizedBase(string message) : base(message)
		{
		}
	}
}
