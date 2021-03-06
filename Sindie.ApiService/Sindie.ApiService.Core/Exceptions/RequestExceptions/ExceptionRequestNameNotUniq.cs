using System;
using System.Collections.Generic;
using System.Text;

namespace Sindie.ApiService.Core.Exceptions.RequestExceptions
{
	/// <summary>
	/// Исключение не уникальное имя в запросе
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ExceptionRequestNameNotUniq<T> : ExceptionApplicationBase
	{
		/// <summary>
		/// Конструктор исключения не уникального имени в запросе
		/// </summary>
		public ExceptionRequestNameNotUniq()
			: base($"{typeof(T)} должен быть уникальным")
		{
		}

		/// <summary>
		/// Конструктор исключения не уникального имени в запросе
		/// </summary>
		/// <param name="name">Имя</param>
		public ExceptionRequestNameNotUniq(string name)
			: base($"{typeof(T)} с именем {name} уже существует.")
		{
		}

		/// <summary>
		/// Конструктор исключения не уникального имени в запросе
		/// </summary>
		/// <param name="name">Имя</param>
		public ExceptionRequestNameNotUniq(string requestName, string name)
			: base($"В запросе {requestName} должно быть уникальное {name}, переданное {name} уже существует")
		{
		}

		/// <summary>
		/// Конструктор исключения не уникального имени в запросе
		/// </summary>
		/// <param name="id">ИД</param>
		public ExceptionRequestNameNotUniq(Guid id)
			: base($"В запросе {typeof(T)} должен быть уникальный ИД, переданный {id} уже существует")
		{
		}
	}
}
