using MediatR;
using Sindie.ApiService.Core.Abstractions;
using System;

namespace Sindie.ApiService.Core.Contracts.AbilityRequests.UpdateAppliedCondition
{
	/// <summary>
	/// Команда создания или изменения накладываемого состояния
	/// </summary>
	public class UpdateAppliedCondionCommand : UpdateAbilityCommandItemAppledCondition, IValidatableCommand
	{
		/// <summary>
		/// Айди способности
		/// </summary>
		public Guid AbilityId { get; set; }

		public void Validate()
		{
			// Method intentionally left empty.
		}
	}
}
