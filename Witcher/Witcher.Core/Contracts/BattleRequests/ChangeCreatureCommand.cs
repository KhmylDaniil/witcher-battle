using System;
using MediatR;

namespace Witcher.Core.Contracts.BattleRequests
{
	/// <summary>
	/// Команда изменения существа в битве
	/// </summary>
	public sealed class ChangeCreatureCommand : IRequest
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
		/// Название существа
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание существа
		/// </summary>
		public string Description { get; set; }
	}
}
