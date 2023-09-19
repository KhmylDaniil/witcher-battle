using MediatR;
using System;

namespace Witcher.Core.Contracts.WeaponTemplateRequests
{
	public sealed class DeleteAppliedConditionForWeaponTemplateCommand : IRequest
	{
		/// <summary>
		/// Айди способности
		/// </summary>
		public Guid WeaponTemplateId { get; set; }

		/// <summary>
		/// Айди
		/// </summary>
		public Guid Id { get; set; }
	}
}
