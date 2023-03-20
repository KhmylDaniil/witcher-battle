using Witcher.Core.Abstractions;
using System;

namespace Witcher.Core.Contracts.AbilityRequests
{
	public class GetAbilityByIdQuery : IValidatableCommand<GetAbilityByIdResponse>
	{
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
