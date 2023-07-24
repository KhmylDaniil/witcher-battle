using MediatR;
using System;

namespace Witcher.Core.Contracts.WeaponTemplateRequests
{
	public sealed class GetWeaponTemplateByIdQuery : IRequest<GetWeaponTemplateByIdResponse>
	{
		public Guid Id { get; set; }
	}
}
