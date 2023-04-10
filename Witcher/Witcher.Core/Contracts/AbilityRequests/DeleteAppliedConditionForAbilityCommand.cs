using Witcher.Core.Abstractions;
using System;

namespace Witcher.Core.Contracts.AbilityRequests
{
	/// <summary>
	/// Команда удаления накладываемого состояния
	/// </summary>
	public class DeleteAppliedConditionForAbilityCommand : IValidatableCommand
	{
		/// <summary>
		/// Айди способности
		/// </summary>
		public Guid AbilityId { get; set; }

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
