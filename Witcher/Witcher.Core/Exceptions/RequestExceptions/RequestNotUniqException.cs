
namespace Witcher.Core.Exceptions.RequestExceptions
{
	/// <summary>
	/// Исключение не уникальный запрос
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class RequestNotUniqException<T> : RequestValidationException
	{
		/// <summary>
		/// Конструктор исключения не уникального запроса
		/// </summary>
		/// <param name="name">Имя</param>
		public RequestNotUniqException(string name)
			: base($"В запросе {typeof(T)} требуются уникальные параметры в поле {name}.")
		{
			UserMessage = $"Требуются уникальные параметры в поле {name}.";
		}
	}
}
