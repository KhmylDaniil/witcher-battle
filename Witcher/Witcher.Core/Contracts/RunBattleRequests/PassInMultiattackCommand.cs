using MediatR;
using System;

namespace Witcher.Core.Contracts.RunBattleRequests
{
	public sealed class PassInMultiattackCommand : IRequest
	{
		/// <summary>
		/// Айди битвы
		/// </summary>
		public Guid BattleId { get; set; }

		/// <summary>
		/// Айди существа
		/// </summary>
		public Guid CreatureId { get; set; }
	}
}
