﻿using System;
using Witcher.Core.Abstractions;
using Witcher.Core.Exceptions.RequestExceptions;

namespace Witcher.Core.Contracts.BagRequests
{
	public class AddItemToBagCommand : IValidatableCommand
	{
		public Guid CharacterId { get; set; }

		public Guid ItemTemplateId { get; set; }

		public int Quantity { get; set; } = 1;

		public void Validate()
		{
			if (Quantity < 1)
				throw new RequestFieldIncorrectDataException<AddItemToBagCommand>(nameof(Quantity), "Значение должно быть больше нуля");
		}
	}
}
