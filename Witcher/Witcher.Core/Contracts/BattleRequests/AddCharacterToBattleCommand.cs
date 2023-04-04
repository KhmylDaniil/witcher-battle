using System;
using Witcher.Core.Abstractions;

namespace Witcher.Core.Contracts.BattleRequests
{
	public class AddCharacterToBattleCommand : IValidatableCommand
	{
		/// <summary>
		/// Айди боя
		/// </summary>
		public Guid BattleId { get; set; }

		/// <summary>
		/// Айди персонажа
		/// </summary>
		public Guid CharacterId { get; set; }

		public void Validate()
		{
			// Method intentionally left empty.
		}
	}
}
