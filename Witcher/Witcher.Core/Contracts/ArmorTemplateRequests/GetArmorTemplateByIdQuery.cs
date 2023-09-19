using MediatR;
using System;

namespace Witcher.Core.Contracts.ArmorTemplateRequests
{
	public sealed class GetArmorTemplateByIdQuery : IRequest<GetArmorTemplateByIdResponse>
	{
		public Guid Id { get; set; }
	}
}
