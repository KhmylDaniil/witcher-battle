using MediatR;
using System;

namespace Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.GetCreatureTemplateById
{
	/// <summary>
	/// Запрос шаблона существа по айди
	/// </summary>
	public class GetCreatureTemplateByIdQuery: IRequest<GetCreatureTemplateByIdResponse>
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
