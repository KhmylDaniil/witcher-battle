namespace Sindie.ApiService.Core.Exceptions
{
	/// <summary>
	/// Исключение нарушение уникальности логина
	/// </summary>
	public class ExceptionLoginUniquenessViolation : ExceptionApplicationBase
	{
		/// <summary>
		/// Конструктор исключения нарушение уникальности логина
		/// </summary>
		/// <param name="name">Имя сущности</param>
		public ExceptionLoginUniquenessViolation(string name)
			: base($"Логин {name} уже существует в системе, введите другой логин.")
		{
		}
	}
}
