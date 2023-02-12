using MediatR;
using System;

namespace Sindie.ApiService.Core.Contracts.AbilityRequests.UpdateAppliedCondition
{
	/// <summary>
	/// Команда создания или изменения накладываемого состояния
	/// </summary>
	public class UpdateAppliedCondionCommand : UpdateAbilityCommandItemAppledCondition, IRequest
	{
		/// <summary>
		/// Айди способности
		/// </summary>
		public Guid AbilityId { get; set; }

		/// <summary>
		/// Айди игры
		/// </summary>
		public Guid GameId { get; set; }
	}
}
