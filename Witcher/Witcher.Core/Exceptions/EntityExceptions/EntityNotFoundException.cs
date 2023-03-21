using System;

namespace Witcher.Core.Exceptions.EntityExceptions
{
	/// <summary>
	/// Исключение не найдена сущность
	/// </summary>
	/// <typeparam name="T">Тип сущности</typeparam>
	public class EntityNotFoundException<T> : EntityBaseException
	{
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