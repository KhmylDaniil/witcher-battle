namespace Sindie.ApiService.Core.Exceptions
{
	/// <summary>
	/// Исключение нет прав доступа к сущности
	/// </summary>
	/// <typeparam name="T">Тип сущности</typeparam>
	public class ExceptionNoAccessToEntity<T> : ExceptionUnauthorizedBase
	{
		/// <summary>
		/// Конструктор исключения нет прав доступа к сущности
		/// </summary>
		public ExceptionNoAccessToEntity()
			: base($"У вас не хватает прав доступа для работы с {typeof(T)}.")
		{
		}

		/// <summary>
		/// Конструктор исключения нет прав доступа к сущности
		/// </summary>
		/// <param name="name"></param>
		public ExceptionNoAccessToEntity(string name)
			: base($"У вас не хватает прав доступа для работы с {name}.")
		{
		}
	}
}
