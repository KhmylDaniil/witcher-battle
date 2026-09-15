using Microsoft.AspNetCore.Http;
using Wastelands.Core.Contracts.Enums;

namespace Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions
{
	public abstract class BusinessLogicException : BaseException
	{
		protected BusinessLogicException(ErrorCode errorCode, string message) : base(errorCode, message)
		{
		}

		public override int StatusCode => StatusCodes.Status400BadRequest;
	}
}
