using Witcher.Core.Abstractions;
using System;

namespace Witcher.Core.Contracts.AbilityRequests
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
