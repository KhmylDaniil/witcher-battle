using MediatR;
using System;

namespace Sindie.ApiService.Core.Contracts.BodyTemplateRequests.GetBodyTemplateById
{
	/// <summary>
	/// Запрос шаблона тела по айди
	/// </summary>
	public class GetBodyTemplateByIdQuery: IRequest<GetBodyTemplateByIdResponse>
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
