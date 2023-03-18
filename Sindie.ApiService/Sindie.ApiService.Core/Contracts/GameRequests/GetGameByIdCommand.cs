using Witcher.Core.Abstractions;
using System;

namespace Witcher.Core.Contracts.GameRequests
{
	public sealed class GetGameByIdCommand : IValidatableCommand<GetGameByIdResponse>
	{
		/// <summary>
		/// Айди
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Валидация
		/// </summary>
		public void Validate()
		{
			// Method intentionally left empty.
		}
	}
}
