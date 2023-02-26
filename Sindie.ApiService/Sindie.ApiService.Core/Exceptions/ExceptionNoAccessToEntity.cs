namespace Sindie.ApiService.Core.Exceptions
{
	/// <summary>
	/// Исключение нет прав доступа к сущности
	/// </summary>
	/// <typeparam name="T">Тип сущности</typeparam>
	public class ExceptionNoAccessToEntity<T> : RequestValidationException
	{
		/// <summary>
		/// Конструктор исключения нет прав доступа к сущности
		/// </summary>
		public ExceptionNoAccessToEntity()
			: base($"У вас не хватает прав доступа для работы с {typeof(T)}.")
		{
		}
	}
}
