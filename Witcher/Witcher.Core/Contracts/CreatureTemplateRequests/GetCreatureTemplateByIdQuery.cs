using System;
using MediatR;

namespace Witcher.Core.Contracts.CreatureTemplateRequests
{
	/// <summary>
	/// Запрос шаблона существа по айди
	/// </summary>
	public sealed class GetCreatureTemplateByIdQuery : IRequest<GetCreatureTemplateByIdResponse>
	{
		/// <summary>
		/// Айди
		/// </summary>
		public Guid Id { get; set; }
	}
}
