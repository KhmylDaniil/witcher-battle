using System;
using System.Collections.Generic;
using System.Text;

namespace Sindie.ApiService.Core.Exceptions.RequestExceptions
{
	/// <summary>
	/// Исключение пустой запрос
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ExceptionRequestNull<T> : ExceptionApplicationBase
	{
		/// <summary>
		/// Конструктор исключения пустого запроса
		/// </summary>
		public ExceptionRequestNull()
			: base($"Пришел пустой запрос {typeof(T)}.")
		{
		}

		/// <summary>
		/// Конструктор исключения пустого запроса
		/// </summary>
		/// <param name="name"></param>
		public ExceptionRequestNull(string name)
			: base($"Пришел пустой запрос {name}.")
		{
		}
	}
}
