using System;

namespace Witcher.Core.Exceptions
{
	/// <summary>
	/// Ошибка логики приложения
	/// </summary>
	public class LogicBaseException : Exception
	{
		public LogicBaseException(string message) : base(message)
		{
		}
	}
}
