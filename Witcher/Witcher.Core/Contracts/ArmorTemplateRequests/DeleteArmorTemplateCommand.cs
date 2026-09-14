using MediatR;
using System;

namespace Witcher.Core.Contracts.ArmorTemplateRequests
{
	public class DeleteArmorTemplateCommand : IRequest<Unit>
	{
		public Guid Id { get; set; }

		public string Name { get; set; }
	}
}
