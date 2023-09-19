using System;
using MediatR;

namespace Witcher.Core.Contracts.BattleRequests
{
	/// <summary>
	/// Запрос получения битвы по айди
	/// </summary>
	public sealed class GetBattleByIdQuery : IRequest<GetBattleByIdResponse>
	{
		/// <summary>
		/// Айди битвы
		/// </summary>
		public Guid Id { get; set; }
	}
}
