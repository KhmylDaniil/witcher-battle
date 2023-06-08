using System;
using Witcher.Core.Abstractions;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.RequestExceptions;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Contracts.ItemRequests
{
	public class CreateItemCommand : IValidatableCommand
	{
		public Guid CharacterId { get; set; }

		public Guid ItemTemplateId { get; set; }

		public int Quantity { get; set; } = 1;

		public void Validate()
		{
			if (Quantity < 1)
				throw new RequestFieldIncorrectDataException<CreateItemCommand>(nameof(Quantity), "Значение должно быть больше нуля");
		}
	}
}
