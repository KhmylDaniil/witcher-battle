using MediatR;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Entities;
using System;

namespace Sindie.ApiService.Core.Contracts.AbilityRequests.GetAbilityById
{
	public class GetAbilityByIdQuery : IValidatableCommand<Ability>
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
