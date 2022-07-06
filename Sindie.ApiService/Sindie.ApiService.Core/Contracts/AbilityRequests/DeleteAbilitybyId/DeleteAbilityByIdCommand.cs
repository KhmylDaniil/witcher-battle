using MediatR;
using System;

namespace Sindie.ApiService.Core.Contracts.AbilityRequests.DeleteAbilitybyId
{
	/// <summary>
	/// Команда на удаление способности по айди
	/// </summary>
	public class DeleteAbilityByIdCommand : IRequest
	{
		/// <summary>
		/// Айди игры
		/// </summary>
		public Guid GameId { get; set; }

		/// <summary>
		/// Айди
		/// </summary>
		public Guid Id { get; set; }
	}
}
