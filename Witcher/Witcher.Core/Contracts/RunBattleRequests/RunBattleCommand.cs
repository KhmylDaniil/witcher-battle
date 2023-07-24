using System;
using MediatR;

namespace Witcher.Core.Contracts.RunBattleRequests
{
	/// <summary>
	/// Команда запуска битвы
	/// </summary>
	public sealed class RunBattleCommand : IRequest<RunBattleResponse>
	{
		/// <summary>
		/// Айди битвы
		/// </summary>
		public Guid BattleId { get; set; }
	}
}
