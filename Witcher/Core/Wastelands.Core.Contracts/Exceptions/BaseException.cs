using Wastelands.Core.Contracts.Enums;

namespace Wastelands.Core.Contracts.Exceptions
{
	public abstract class BaseException : Exception
	{
		public ErrorCode ErrorCode { get; }

		public abstract int StatusCode { get; }

		public string? Description { get; protected set; }

		protected BaseException(ErrorCode errorCode, string message) : base(message)
		{
			// TODO: add validation.
			ErrorCode = errorCode;
		}
	}
}
