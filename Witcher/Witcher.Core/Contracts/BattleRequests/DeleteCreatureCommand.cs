using System;
using MediatR;

namespace Witcher.Core.Contracts.BattleRequests
{
	/// <summary>
	/// Команда удаления существа
	/// </summary>
	public sealed class DeleteCreatureCommand : IRequest
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
		/// Название
		/// </summary>
		public string Name { get; set; }
	}
}
