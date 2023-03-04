using System;

namespace Sindie.ApiService.Core.Exceptions
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
