using System;
using System.Collections.Generic;
using System.Text;

namespace Sindie.ApiService.Core.Exceptions.RequestExceptions
{
	/// <summary>
	/// Исключение не уникальный запрос
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ExceptionRequestNotUniq<T> : ExceptionApplicationBase
	{
		/// <summary>
		/// Конструктор исключения не уникального запроса
		/// </summary>
		public ExceptionRequestNotUniq()
			: base($"В запросе {typeof(T)} должны быть уникальные параметры")
		{
		}

		/// <summary>
		/// Конструктор исключения не уникального запроса
		/// </summary>
		/// <param name="name">Имя</param>
		public ExceptionRequestNotUniq(string name)
			: base($"В запросе {typeof(T)} должно быть уникальное поле, переданное {name} уже существует")
		{
		}

		/// <summary>
		/// Конструктор исключения не уникального запроса
		/// </summary>
		/// <param name="name">Имя</param>
		public ExceptionRequestNotUniq(string requestName, string name)
			: base($"В запросе {requestName} должно быть уникальное поле, переданное {name} уже существует")
		{
		}

		/// <summary>
		/// Конструктор исключения не уникального запроса
		/// </summary>
		/// <param name="id">ИД</param>
		public ExceptionRequestNotUniq(Guid id)
			: base($"В запросе {typeof(T)} должен быть уникальный ИД, переданный {id} уже существует")
		{
		}
	}
}
