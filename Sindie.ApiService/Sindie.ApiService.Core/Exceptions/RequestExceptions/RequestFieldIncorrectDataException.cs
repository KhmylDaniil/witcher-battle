
using System;

namespace Sindie.ApiService.Core.Exceptions.RequestExceptions
{
	/// <summary>
	/// Исключение в поле запроса задано некорректное значение
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class RequestFieldIncorrectDataException<T> : RequestValidationException
	{
		/// <summary>
		/// Конструктор в поле запроса задано некорректное значение
		/// </summary>
		public RequestFieldIncorrectDataException()
			: base($"В запросе {typeof(T)} в поле или нескольких полях задано некорректное значение.")
		{
			UserMessage = "В поле или нескольких полях задано некорректное значение";
		}

		/// <summary>
		/// Конструктор исключения в поле запроса задано некорректное значение 
		/// </summary>
		public RequestFieldIncorrectDataException(string name)
			: base($"В запросе {typeof(T)} в поле {name} задано некорректное значение.")
		{
			UserMessage = $"В поле {name} задано некорректное значение.";
		}

		/// <summary>
		/// Конструктор исключения в поле запроса задано некорректное значение
		/// </summary>
		public RequestFieldIncorrectDataException(string name, string message)
			: base($"В запросе {typeof(T)} в поле {name} задано некорректное значение. {message}.")
		{
			UserMessage = $"В поле {name} задано некорректное значение. {message}.";
		}
	}
}
