using Witcher.Core.Abstractions;
using System;

namespace Witcher.Core.Contracts.RunBattleRequests
{
	/// <summary>
	/// Команда попытки снятия эффекта
	/// </summary>
	public class HealEffectCommand : IValidatableCommand
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
		/// Айди цели
		/// </summary>
		public Guid TargetCreatureId { get; set; }

		/// <summary>
		/// Айди эффекта
		/// </summary>
		public Guid EffectId { get; set; }

		/// <summary>
		/// Валидация
		/// </summary>
		public void Validate()
		{
			// Method intentionally left empty.
		}
	}
}
