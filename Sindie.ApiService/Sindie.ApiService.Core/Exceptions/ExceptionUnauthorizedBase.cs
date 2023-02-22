using System;
using System.Collections.Generic;
using System.Text;

namespace Sindie.ApiService.Core.Exceptions
{
	/// <summary>
	/// Базовое исключение авторизации (код 403 Forbidden) 
	/// </summary>
	public class ExceptionUnauthorizedBase : ExceptionApplicationBase
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
