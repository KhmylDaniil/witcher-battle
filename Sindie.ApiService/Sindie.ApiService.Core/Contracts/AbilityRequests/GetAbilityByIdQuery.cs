using Sindie.ApiService.Core.Abstractions;
using System;

namespace Sindie.ApiService.Core.Contracts.AbilityRequests
{
	public class GetAbilityByIdQuery : IValidatableCommand<GetAbilityByIdResponse>
	{
		public Guid Id { get; set; }

		public Guid GameId { get; set; }

		/// <summary>
		/// Валидация
		/// </summary>
		public void Validate()
		{
			// Method intentionally left empty.
		}
	}
}
