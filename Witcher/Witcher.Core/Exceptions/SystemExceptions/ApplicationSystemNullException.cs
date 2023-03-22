namespace Witcher.Core.Exceptions.SystemExceptions
{
	public class ApplicationSystemNullException<T>: ApplicationSystemBaseException
	{
		public ApplicationSystemNullException(string argument)
			: base($"При работе класса {typeof(T)} отсутствует необходимый параметр {argument}.")
		{
		}
	}
}
