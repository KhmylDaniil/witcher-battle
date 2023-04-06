using System;
using Witcher.Core.Abstractions;

namespace Witcher.Core.Contracts.RunBattleRequests
{
	public class PassInMultiattackCommand : IValidatableCommand
	{
		/// <summary>
		/// Айди битвы
		/// </summary>
		public Guid BattleId { get; set; }

		/// <summary>
		/// Айди существа
		/// </summary>
		public Guid CreatureId { get; set; }

		/// <summary>
		/// Валидация
		/// </summary>
		public void Validate()
		{
			// Method intentionally left empty.
		}
	}
}
