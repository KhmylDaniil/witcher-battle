using System;
using Witcher.Core.Abstractions;

namespace Witcher.Core.Contracts.WeaponTemplateRequests
{
	public class DeleteAppliedConditionForWeaponTemplateCommand : IValidatableCommand
	{
		/// <summary>
		/// Айди способности
		/// </summary>
		public Guid WeaponTemplateId { get; set; }

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
