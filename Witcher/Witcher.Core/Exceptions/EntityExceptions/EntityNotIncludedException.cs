namespace Witcher.Core.Exceptions.EntityExceptions
{
	public class EntityNotIncludedException<T> : EntityBaseException
	{
		public EntityNotIncludedException(string argumentName)
			: base($"К сущности {typeof(T)} не присоединена сущность  {argumentName}.") { }
	}
}
