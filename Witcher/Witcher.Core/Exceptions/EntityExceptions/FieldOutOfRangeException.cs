using System;

namespace Witcher.Core.Exceptions.EntityExceptions
{
	/// <summary>
	/// Исключение неверные данные в поле сущности
	/// </summary>
	/// <typeparam name="T">Сущность</typeparam>
	public class FieldOutOfRangeException<T>: ArgumentOutOfRangeException
	{
		/// <summary>
		/// Конструктор исключения неверные данные в поле сущности
		/// </summary>
		public FieldOutOfRangeException(string name)
			: base($"В сущности {typeof(T)} в поле {name} задано некорректное значение.")
		{
		}
	}
}
