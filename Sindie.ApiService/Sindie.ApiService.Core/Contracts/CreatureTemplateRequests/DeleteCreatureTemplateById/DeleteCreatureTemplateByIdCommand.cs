using MediatR;
using System;

namespace Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.DeleteCreatureTemplateById
{
	/// <summary>
	/// Команда на удаление шаблона существа по айди
	/// </summary>
	public sealed class DeleteCreatureTemplateByIdCommand: IRequest
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
