using MediatR;
using System;

namespace Witcher.Core.Contracts.BattleRequests
{
	public class AddCharacterToBattleCommand : IRequest
	{
		/// <summary>
		/// Айди боя
		/// </summary>
		public Guid BattleId { get; set; }

		/// <summary>
		/// Айди персонажа
		/// </summary>
		public Guid CharacterId { get; set; }
	}
}
