using System;
using System.Collections.Generic;
using System.Text;

namespace Sindie.ApiService.Core.Exceptions.EntityExceptions
{
	/// <summary>
	/// Исключение сущность not included
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ExceptionEntityNotIncluded<T> : ExceptionNotFoundBase
	{
		/// <summary>
		/// Конструктор исключения сущность not included
		/// </summary>
		public ExceptionEntityNotIncluded()
			//TODO to do придумать текст получше
			: base($"Не присоединена (сущность {typeof(T)} not included)")
		{
		}

		/// <summary>
		/// Конструктор исключения сущность не присоединена
		/// </summary>
		/// <param name="name">Название</param>
		/// <param name="guid">Айди</param>
		public ExceptionEntityNotIncluded(string name, Guid guid)
			//TODO to do придумать текст получше
			: base($"Сущность типа {typeof(T)} не присоединена к сущности {name} с айди {guid}")
		{
		}

		/// <summary>
		/// Конструктор исключения сущность не присоединена
		/// </summary>
		/// <param name="name"></param>
		public ExceptionEntityNotIncluded(string name)
			//TODO to do придумать текст получше
			: base($"Сущность типа {typeof(T)} не присоединена к сущности {name}")
		{
		}
	}
}
