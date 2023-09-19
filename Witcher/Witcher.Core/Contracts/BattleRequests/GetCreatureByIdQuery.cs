using System;
using MediatR;

namespace Witcher.Core.Contracts.BattleRequests
{
	public sealed class GetCreatureByIdQuery : IRequest<GetCreatureByIdResponse>
	{
		/// <summary>
		/// Айди битвы
		/// </summary>
		public Guid BattleId { get; set; }

		/// <summary>
		/// Айди
		/// </summary>
		public Guid Id { get; set; }
	}
}
