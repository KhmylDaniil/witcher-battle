using MediatR;
using System;

namespace Witcher.Core.Contracts.WeaponTemplateRequests
{
	public sealed class DeleteWeaponTemplateCommand : IRequest<Unit>
	{
		public Guid Id { get; set; }

		public string Name { get; set; }
	}
}
