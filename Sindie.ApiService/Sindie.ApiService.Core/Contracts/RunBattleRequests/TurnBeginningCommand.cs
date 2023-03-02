using Sindie.ApiService.Core.Abstractions;
using System;

namespace Sindie.ApiService.Core.Contracts.RunBattleRequests
{
	/// <summary>
	/// Запрос начала хода
	/// </summary>
	public sealed class TurnBeginningCommand : IValidatableCommand<TurnBeginningResponse>
	{
		/// <summary>
		/// Айди боя
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
