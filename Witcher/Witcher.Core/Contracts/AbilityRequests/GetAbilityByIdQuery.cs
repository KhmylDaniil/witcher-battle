using System;
using MediatR;

namespace Witcher.Core.Contracts.AbilityRequests
{
	public sealed class GetAbilityByIdQuery : IRequest<GetAbilityByIdResponse>
	{
		public Guid Id { get; set; }
	}
}
