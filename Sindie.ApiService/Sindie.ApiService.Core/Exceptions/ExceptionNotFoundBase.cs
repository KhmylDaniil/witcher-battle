namespace Sindie.ApiService.Core.Exceptions
{
	/// <summary>
	/// Исключение ничего не найдено (код 404 Not Found)
	/// </summary>
	public class ExceptionNotFoundBase : ExceptionApplicationBase
	{
		/// <summary>
		/// Конструктор исключения ничего не найдено
		/// </summary>
		/// <param name="message">Текст исключения</param>
		public ExceptionNotFoundBase(string message) : base(message)
		{
		}
	}
}
