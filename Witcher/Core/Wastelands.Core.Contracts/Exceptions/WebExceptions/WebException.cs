using Wastelands.Core.Contracts.Enums;

namespace Wastelands.Core.Contracts.Exceptions.WebExceptions
{
	public abstract class WebException : BaseException
	{
		protected WebException(ErrorCode errorCode, string message) : base(errorCode, message)
		{
		}
	}
}
