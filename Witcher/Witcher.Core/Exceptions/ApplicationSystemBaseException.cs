using System;

namespace Witcher.Core.Exceptions
{
	/// <summary>
	/// Предвиденная ошибка вне бизнес-логики приложения 
	/// </summary>
	public class ApplicationSystemBaseException : Exception
	{
		public ApplicationSystemBaseException(string message) : base(message)
		{
		}
	}
}
