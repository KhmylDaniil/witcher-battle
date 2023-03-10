using Sindie.ApiService.Core.Abstractions;
using System;

namespace Sindie.ApiService.Core.Contracts.RunBattleRequests
{
	/// <summary>
	/// Команда запуска битвы
	/// </summary>
	public class MakeTurnCommand : IValidatableCommand<MakeTurnResponse>
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
