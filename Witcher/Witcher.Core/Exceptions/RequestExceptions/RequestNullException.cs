
namespace Witcher.Core.Exceptions.RequestExceptions
{
	/// <summary>
	/// Исключение пустой запрос
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class RequestNullException<T> : RequestValidationException
	{
		/// <summary>
		/// Конструктор исключения пустого запроса
		/// </summary>
		public RequestNullException()
			: base($"Пришел пустой запрос {typeof(T)}.")
		{
		}

		/// <summary>
		/// Конструктор исключения пустого запроса
		/// </summary>
		/// <param name="name"></param>
		public RequestNullException(string name)
			: base($"Пришел пустой запрос {name}.")
		{
		}
	}
}
