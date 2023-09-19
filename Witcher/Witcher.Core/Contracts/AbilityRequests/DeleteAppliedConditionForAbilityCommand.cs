using System;
using MediatR;

namespace Witcher.Core.Contracts.AbilityRequests
{
	/// <summary>
	/// Команда удаления накладываемого состояния
	/// </summary>
	public sealed class DeleteAppliedConditionForAbilityCommand : IRequest
	{
		/// <summary>
		/// Айди способности
		/// </summary>
		public Guid AbilityId { get; set; }

		/// <summary>
		/// Айди
		/// </summary>
		public Guid Id { get; set; }
	}
}
