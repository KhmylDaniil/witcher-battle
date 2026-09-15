using Microsoft.AspNetCore.Http;
using Wastelands.Core.Contracts.Enums;

namespace Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions
{
	public class NotFoundException : BusinessLogicException
	{
		public NotFoundException(ErrorCode errorCode, string entityType, string keyName, string key) :
			base(errorCode, $"{entityType} was not found by {keyName} = '{key}'.")
		{
		}

		public NotFoundException(ErrorCode errorCode, string message) :
			base(errorCode, message)
		{
		}

		public override int StatusCode => StatusCodes.Status404NotFound;

		public static void ThrowIfNull<T>(
			T? entity,
			ErrorCode errorCode,
			string entityType,
			string keyName,
			string key)
			where T : class
		{
			if (entity is null)
			{
				throw new NotFoundException(errorCode, entityType, keyName, key);
			}
		}
	}
}
