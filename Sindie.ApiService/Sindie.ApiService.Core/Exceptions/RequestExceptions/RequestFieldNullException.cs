
namespace Sindie.ApiService.Core.Exceptions.RequestExceptions
{
	/// <summary>
	/// Исключение пустое поле в запросе
	/// </summary>
	public class RequestFieldNullException<T> : RequestValidationException
	{
		/// <summary>
		/// Конструктор исключения пустое поле в запросе
		/// </summary>
		public RequestFieldNullException(string name)
			: base($"В запросе {typeof(T)} не заполнено обязательное поле {name}")
		{
			UserMessage = $"Не заполнено обязательное поле {name}.";
		}
	}
}
