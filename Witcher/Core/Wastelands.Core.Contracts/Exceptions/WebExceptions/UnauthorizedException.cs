using Microsoft.AspNetCore.Http;
using Wastelands.Core.Contracts.Enums;

namespace Wastelands.Core.Contracts.Exceptions.WebExceptions
{
	public class UnauthorizedException : WebException
	{
		public UnauthorizedException(ErrorCode errorCode, string message) : base(errorCode, message)
		{
		}

		public override int StatusCode => StatusCodes.Status401Unauthorized;
	}
}
