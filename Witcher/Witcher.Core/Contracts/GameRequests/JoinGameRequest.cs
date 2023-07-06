using System;
using Witcher.Core.Abstractions;
using Witcher.Core.Exceptions.RequestExceptions;

namespace Witcher.Core.Contracts.GameRequests
{
	public class JoinGameRequest : IValidatableCommand
	{
		public Guid UserId { get; set; }

		public Guid GameId { get; set; }

		public string Message { get; set; }

		public void Validate()
		{
			if (Message?.Length > 100)
				throw new RequestFieldIncorrectDataException<JoinGameRequest>(nameof(Message), "Длина сообщения превышает 100 символов.");
		}
	}
}
