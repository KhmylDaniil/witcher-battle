using MediatR;
using System;

namespace Witcher.Core.Contracts.ArmorTemplateRequests
{
	public class DeleteArmorTemplateCommand : IRequest
	{
		public Guid Id { get; set; }

		public string Name { get; set; }
	}
}
