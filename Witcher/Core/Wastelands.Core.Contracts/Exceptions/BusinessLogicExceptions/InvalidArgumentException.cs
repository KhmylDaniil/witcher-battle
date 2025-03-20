using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Wastelands.Core.Contracts.Constants;
using Wastelands.Core.Contracts.Enums;

namespace Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions
{
	public class InvalidArgumentException : BusinessLogicException
	{
		public InvalidArgumentException(ErrorCode errorCode, string message) : base(errorCode, message)
		{
		}

		public static void ThrowIfLessOrEqualToZero<T>(T value, ErrorCode errorCode, string paramName)
			where T : struct, INumber<T>
		{
			if (value <= T.Zero)
			{
				throw new InvalidArgumentException(errorCode, string.Format(ExceptionMessages.ValueMustBePositive, paramName));
			}
		}

		public static void ThrowIfLessOrEqualToZero<T>(T value, string paramName)
			where T : struct, INumber<T>
		{
			if (value <= T.Zero)
			{
				throw new InvalidArgumentException(ErrorCode.PropertyIsLessOrEqualToZero,
					string.Format(ExceptionMessages.ValueMustBePositive, paramName));
			}
		}

		public static void ThrowIfLessOrEqualToZero<T>(T? value, string paramName)
			where T : struct, INumber<T>
		{
			if (value <= T.Zero)
			{
				throw new InvalidArgumentException(ErrorCode.PropertyIsLessOrEqualToZero,
					string.Format(ExceptionMessages.ValueMustBePositive, paramName));
			}
		}

		public static void ThrowIfNullOrLessOrEqualToZero<T>(T? value, string paramName)
			where T : struct, INumber<T>
		{
			if (!value.HasValue || value <= T.Zero)
			{
				throw new InvalidArgumentException(ErrorCode.PropertyIsLessOrEqualToZero,
					string.Format(ExceptionMessages.ValueMustBePositive, paramName));
			}
		}

		public static void ThrowIfNullOrLessOrEqualToZero<T>(T? value, ErrorCode errorCode, string paramName)
			where T : struct, INumber<T>
		{
			if (!value.HasValue || value <= T.Zero)
			{
				throw new InvalidArgumentException(errorCode,
					string.Format(ExceptionMessages.ValueMustBePositive, paramName));
			}
		}

		public static void ThrowIfLessThanZero<T>(T value, string paramName)
			where T : struct, INumber<T>
		{
			if (value < T.Zero)
			{
				throw new InvalidArgumentException(ErrorCode.PropertyIsNullOrLessThanZero,
					string.Format(ExceptionMessages.ValueMustBePositive, paramName));
			}
		}

		public static void ThrowIfLessThanZero<T>(T? value, string paramName)
			where T : struct, INumber<T>
		{
			if (value < T.Zero)
			{
				throw new InvalidArgumentException(ErrorCode.PropertyIsNullOrLessThanZero,
					string.Format(ExceptionMessages.ValueMustBePositive, paramName));
			}
		}

		public static void ThrowIfNullOrLessThanZero<T>(T? value, string paramName)
			where T : struct, INumber<T>
		{
			if (!value.HasValue || value < T.Zero)
			{
				throw new InvalidArgumentException(ErrorCode.PropertyIsNullOrLessThanZero,
					string.Format(ExceptionMessages.ValueMustBePositive, paramName));
			}
		}

		public static void ThrowIfNullOrWhiteSpace(
			string? value,
			string? paramName,
			ErrorCode errorCode = ErrorCode.RequiredParameterCannotBeNull)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				throw new InvalidArgumentException(
					errorCode,
					string.Format(ExceptionMessages.FieldCantBeEmpty, paramName));
			}
		}

		public static void ThrowIfNull<T>(
			T? value,
			ErrorCode errorCode,
			string? paramName) where T : struct
		{
			if (!value.HasValue)
			{
				throw new InvalidArgumentException(
					errorCode,
					string.Format(ExceptionMessages.FieldCantBeEmpty, paramName));
			}
		}

		public static void ThrowIfNull<T>(
			T? value,
			string? paramName,
			ErrorCode errorCode = ErrorCode.RequiredParameterCannotBeNull) where T : struct
		{
			if (!value.HasValue)
			{
				throw new InvalidArgumentException(
					errorCode,
					string.Format(ExceptionMessages.FieldCantBeEmpty, paramName));
			}
		}

		public static void ThrowIfDateNullOrDefault(
			DateOnly? value,
			string? paramName,
			ErrorCode errorCode = ErrorCode.RequiredParameterCannotBeNull)
		{
			if (!value.HasValue || value.Value == default)
			{
				throw new InvalidArgumentException(
					errorCode,
					string.Format(ExceptionMessages.FieldCantBeEmpty, paramName));
			}
		}

		public static void ThrowIfDateTimeNullOrDefault(
			DateTime? value,
			string? paramName,
			ErrorCode errorCode = ErrorCode.RequiredParameterCannotBeNull)
		{
			if (!value.HasValue || value.Value == default)
			{
				throw new InvalidArgumentException(
					errorCode,
					string.Format(ExceptionMessages.FieldCantBeEmpty, paramName));
			}
		}

		public static void ThrowIfNull<T>(
			T? value,
			string? paramName,
			ErrorCode errorCode = ErrorCode.RequiredParameterCannotBeNull) where T : class
		{
			if (value is null)
			{
				throw new InvalidArgumentException(
					errorCode,
					string.Format(ExceptionMessages.FieldCantBeEmpty, paramName));
			}
		}

		public static void ThrowIfNullOrEmpty<T>(
			IEnumerable<T>? items,
			string? paramName,
			ErrorCode errorCode = ErrorCode.InvalidArgument)
		{
			if (items is null || !items.Any())
			{
				throw new InvalidArgumentException(
					errorCode,
					string.Format(ExceptionMessages.FieldCantBeEmpty, paramName));
			}
		}
	}
}
