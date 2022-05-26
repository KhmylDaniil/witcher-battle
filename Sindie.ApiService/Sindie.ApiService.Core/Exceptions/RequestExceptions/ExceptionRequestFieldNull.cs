using System;
using System.Collections.Generic;
using System.Text;

namespace Sindie.ApiService.Core.Exceptions.RequestExceptions
{
	/// <summary>
	/// Исключение пустое поле в запросе
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ExceptionRequestFieldNull<T> : ExceptionApplicationBase
	{
		/// <summary>
		/// Конструктор исключения пустое поле в запросе
		/// </summary>
		public ExceptionRequestFieldNull(string name)
			: base($"В запросе {typeof(T)} не заполнено обязательное поле {name}")
		{
		}
	}
}
