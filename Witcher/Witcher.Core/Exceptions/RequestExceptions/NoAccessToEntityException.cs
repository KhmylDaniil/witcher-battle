namespace Witcher.Core.Exceptions.RequestExceptions
{
	/// <summary>
	/// Исключение нет прав доступа к сущности
	/// </summary>
	/// <typeparam name="T">Тип сущности</typeparam>
	public class NoAccessToEntityException<T> : RequestValidationException
	{
		/// <summary>
		/// Конструктор исключения нет прав доступа к сущности
		/// </summary>
		public NoAccessToEntityException()
			: base($"У вас не хватает прав доступа для работы с {typeof(T)}.")
		{
		}
	}
}
