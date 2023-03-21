using System;

namespace Witcher.Core.Exceptions
{
	/// <summary>
	/// Исключение ничего не найдено (код 404 Not Found)
	/// </summary>
	public class NotFoundBaseException : ArgumentException
	{
		/// <summary>
		/// Конструктор исключения ничего не найдено
		/// </summary>
		/// <param name="message">Текст исключения</param>
		public NotFoundBaseException(string message) : base(message)
		{
		}
	}
}
