using MediatR;
using System;

namespace Sindie.ApiService.Core.Contracts.BattleRequests.TreatEffect
{
	/// <summary>
	/// Команда попытки снятия эффекта
	/// </summary>
	public sealed class TreatEffectCommand : IRequest<TreatEffectResponse>
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
		/// Айди эффекта
		/// </summary>
		public Guid EffectId { get; set; }
	}
}
