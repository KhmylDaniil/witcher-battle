
namespace Sindie.ApiService.Core.Exceptions.RequestExceptions
{
	/// <summary>
	/// Исключение в поле запроса задано некорректное значение
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ExceptionRequestFieldIncorrectData<T> : ExceptionApplicationBase
	{
		/// <summary>
		/// Конструктор в поле запроса задано некорректное значение
		/// </summary>
		public ExceptionRequestFieldIncorrectData()
			: base($"В запросе {typeof(T)} в поле или нескольких полях задано некорректное значение.")
		{
		}

		/// <summary>
		/// Конструктор исключения в поле запроса задано некорректное значение 
		/// </summary>
		public ExceptionRequestFieldIncorrectData(string name)
			: base($"В запросе {typeof(T)} в поле {name} задано некорректное значение.")
		{
		}

		/// <summary>
		/// Конструктор исключения в поле запроса задано некорректное значение
		/// </summary>
		public ExceptionRequestFieldIncorrectData(string name, string text)
			: base($"В запросе {typeof(T)} в поле {name} задано некорректное значение, значение должно соответствовать {text}.")
		{
		}
	}
}
