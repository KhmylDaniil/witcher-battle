using System;

namespace Witcher.Core.Exceptions.EntityExceptions
{
	/// <summary>
	/// Исключение не найдена сущность
	/// </summary>
	/// <typeparam name="T">Тип сущности</typeparam>
	public class EntityNotFoundException<T> : NotFoundBaseException
	{
		/// <summary>
		/// Конструктор исключения не найдена сущность
		/// </summary>
		public EntityNotFoundException()
			: base($"Не найдена сущность {typeof(T)}")
		{
		}

		/// <summary>
		/// Конструктор исключения не найдена сущность
		/// </summary>
		/// <param name="name">Название сущности</param>
		public EntityNotFoundException(string name)
			: base($"Не найдена сущность {typeof(T)} с именем {name}")
		{
		}

		/// <summary>
		/// Конструктор исключения не найдена сущность
		/// </summary>
		/// <param name="name">Название сущности</param>
		/// <param name="id">ИД сущности</param>
		public EntityNotFoundException(string name, Guid id)
			: base($"Не найдена сущность {typeof(T)} {name} с ИД {id}")
		{
		}

		/// <summary>
		/// Конструктор исключения не найдена сущность
		/// </summary>
		/// <param name="id">ИД сущности</param>
		public EntityNotFoundException(Guid id)
			: base($"Не найдена сущность {typeof(T)} с ИД {id}")
		{
		}
	}
}