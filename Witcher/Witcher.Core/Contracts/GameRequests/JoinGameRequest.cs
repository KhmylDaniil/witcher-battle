using MediatR;
using System;

namespace Witcher.Core.Contracts.GameRequests
{
	public sealed class JoinGameRequest : IRequest
	{
		public Guid UserId { get; set; }

		public Guid GameId { get; set; }

		public string Message { get; set; }
	}
}
