using System;
using System.Collections.Generic;
using System.Text;

namespace Sindie.ApiService.Core.Exceptions
{
	/// <summary>
	/// Базовое исключение скрипта (код 420) код ошибки с потолка 
	/// </summary>
	public class ExceptionScryptBase : ArgumentException
	{
		/// <summary>
		/// Конструктор базового исключения скрипта
		/// </summary>
		/// <param name="message">Текст исключения</param>
		public ExceptionScryptBase(string message) : base(message)
		{
		}
	}
}
