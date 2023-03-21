using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		public FieldOutOfRangeException()
			: base($"В сущности {typeof(T)} в поле или нескольких полях задано некорректное значение.")
		{
		}

		/// <summary>
		/// Конструктор исключения неверные данные в поле сущности
		/// </summary>
		public FieldOutOfRangeException(string name)
			: base($"В сущности {typeof(T)} в поле {name} задано некорректное значение.")
		{
		}

		/// <summary>
		/// Конструктор исключения неверные данные в поле сущности
		/// </summary>
		public FieldOutOfRangeException(string name, string text)
			: base($"В сущности {typeof(T)} в поле {name} задано некорректное значение, значение должно соответствовать {text}.")
		{
		}
	}
}
