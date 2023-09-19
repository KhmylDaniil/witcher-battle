using System;
using MediatR;

namespace Witcher.Core.Contracts.RunBattleRequests
{
	/// <summary>
	/// Команда попытки снятия эффекта
	/// </summary>
	public class HealEffectCommand : IRequest
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
		public Guid TargetId { get; set; }

		/// <summary>
		/// Айди эффекта
		/// </summary>
		public Guid EffectId { get; set; }
	}
}
