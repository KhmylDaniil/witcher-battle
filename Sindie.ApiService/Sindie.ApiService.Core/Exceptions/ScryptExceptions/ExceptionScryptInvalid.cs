using System;
using System.Collections.Generic;
using System.Text;

namespace Sindie.ApiService.Core.Exceptions.ScryptExceptions
{
	/// <summary>
	/// Исключение не валидный скрипт
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ExceptionScryptInvalid<T> : ExceptionScryptBase
	{
		/// <summary>
		/// Конструктор исключения не валидного скрипта
		/// </summary>
		public ExceptionScryptInvalid()
				: base($"Не валидный скрипт {typeof(T)}.")
		{
		}

		/// <summary>
		/// Конструктор исключения не валидного скрипта
		/// </summary>
		/// <param name="name">Название скрипта</param>
		public ExceptionScryptInvalid(string name) 
				: base($"Не валидный скрипт {name}.")
		{
		}
	}
}
