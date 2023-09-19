using System;
using MediatR;

namespace Witcher.Core.Contracts.BodyTemplateRequests
{
	/// <summary>
	/// Запрос шаблона тела по айди
	/// </summary>
	public class GetBodyTemplateByIdQuery : IRequest<GetBodyTemplateByIdResponse>
	{
		/// <summary>
		/// Айди
		/// </summary>
		public Guid Id { get; set; }
	}
}
