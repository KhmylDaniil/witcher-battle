using System;
using MediatR;

namespace Witcher.Core.Contracts.GameRequests
{
	public sealed class GetGameByIdCommand : IRequest<GetGameByIdResponse>
	{
		/// <summary>
		/// Айди
		/// </summary>
		public Guid Id { get; set; }
	}
}
