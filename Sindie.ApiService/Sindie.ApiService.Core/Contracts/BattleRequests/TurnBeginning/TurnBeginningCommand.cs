using MediatR;
using System;

namespace Sindie.ApiService.Core.Contracts.BattleRequests.TurnBeginning
{
	/// <summary>
	/// Запрос начала хода
	/// </summary>
	public sealed class TurnBeginningCommand : IRequest<TurnBeginningResponse>
	{
		/// <summary>
		/// Айди боя
		/// </summary>
		public Guid BattleId { get; set; }

		/// <summary>
		/// Айди существа
		/// </summary>
		public Guid CreatureId { get; set; }
	}
}
