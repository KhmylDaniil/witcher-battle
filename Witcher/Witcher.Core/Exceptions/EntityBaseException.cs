using System;

namespace Witcher.Core.Exceptions
{
	/// <summary>
	/// Базовое исключение сущности в игре
	/// </summary>
	public class EntityBaseException : Exception
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="message">Текст исключения</param>
		public EntityBaseException(string message) : base(message)
		{
		}
	}
}
