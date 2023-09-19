using System;
using MediatR;

namespace Witcher.Core.Contracts.BattleRequests
{
	/// <summary>
	/// Команда добавлени существа в битву
	/// </summary>
	public class CreateCreatureCommand : IRequest
	{
		/// <summary>
		/// Айди боя
		/// </summary>
		public Guid BattleId { get; set; }

		/// <summary>
		/// Айди шаблона существа
		/// </summary>
		public Guid CreatureTemplateId { get; set; }

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
