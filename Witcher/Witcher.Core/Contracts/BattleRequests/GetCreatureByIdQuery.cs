using Witcher.Core.Abstractions;
using System;

namespace Witcher.Core.Contracts.BattleRequests
{
	public class GetCreatureByIdQuery : IValidatableCommand<GetCreatureByIdResponse>
	{
		/// <summary>
		/// Айди битвы
		/// </summary>
		public Guid BattleId { get; set; }

		/// <summary>
		/// Айди
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Валидация
		/// </summary>
		public void Validate()
		{
			// Method intentionally left empty.
		}
	}
}
