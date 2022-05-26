using System;
using System.Collections.Generic;
using System.Text;

namespace Sindie.ApiService.Core.Exceptions.ScryptExceptions
{
	//TODO to do доделать когда появятся скрипты
	//Скрипт (название скрипта) не может работать с переданными значениями (список название - значение).
	//Передать Dictionary<string, string> в параметры конструктора ошибки и в цикле вывести эти стринги пользователю
	/// <summary>
	/// Исключение cкрипт не может работать с переданными значениями
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ExceptionScryptInvalidValue<T> : ExceptionScryptBase
	{
		/// <summary>
		/// Конструктор исключения cкрипт не может работать с переданными значениями
		/// </summary>
		public ExceptionScryptInvalidValue()
				: base($"Скрипт {typeof(T)} не может работать с переданными значениями.")
		{
		}
	}
}
