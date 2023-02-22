using System;
using System.Collections.Generic;
using System.Text;

namespace Sindie.ApiService.Core.Exceptions.ScryptExceptions
{
	/// <summary>
	/// Исключение не удалось интерпретировать скрипт
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ExceptionScryptCannotInterpret<T> : ExceptionScryptBase
	{
		/// <summary>
		/// Конструктор исключения не удалось интерпретировать скрипт
		/// </summary>
		public ExceptionScryptCannotInterpret()
				: base($"Не удалось интерпретировать скрипт {typeof(T)}.")
		{
		}

		/// <summary>
		/// Конструктор исключения не удалось интерпретировать скрипт
		/// </summary>
		/// <param name="name">Название скрипта</param>
		public ExceptionScryptCannotInterpret(string name)
				: base($"Не удалось интерпретировать скрипт {name}.")
		{
		}
	}
}
