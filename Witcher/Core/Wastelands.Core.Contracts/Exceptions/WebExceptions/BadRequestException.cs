using Microsoft.AspNetCore.Http;
using Wastelands.Core.Contracts.Enums;

namespace Wastelands.Core.Contracts.Exceptions.WebExceptions
{
	public class BadRequestException : WebException
	{
		public BadRequestException(ErrorCode errorCode, string message) : base(errorCode, message)
		{
		}

		public override int StatusCode => StatusCodes.Status400BadRequest;
	}
}
