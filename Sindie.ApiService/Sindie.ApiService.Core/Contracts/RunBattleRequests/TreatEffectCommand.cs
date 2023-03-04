using Sindie.ApiService.Core.Abstractions;
using System;

namespace Sindie.ApiService.Core.Contracts.RunBattleRequests
{
	/// <summary>
	/// Команда попытки снятия эффекта
	/// </summary>
	public sealed class TreatEffectCommand : IValidatableCommand<TreatEffectResponse>
	{
		/// <summary>
		/// Айди боя
		/// </summary>
		public Guid BattleId { get; set; }

		/// <summary>
		/// Айди существа
		/// </summary>
		public Guid Id { get; set; }

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
