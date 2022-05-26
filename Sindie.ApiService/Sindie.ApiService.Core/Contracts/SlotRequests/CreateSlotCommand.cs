using MediatR;
using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Contracts.SlotRequests
{
	/// <summary>
	/// Команда создания слота
	/// </summary>
	public class CreateSlotCommand : IRequest<CreateSlotResponse>
	{
		/// <summary>
		/// Айди игры
		/// </summary>
		public Guid GameId { get; set; }

		/// <summary>
		/// Название слота
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание слота
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Список предметов
		/// </summary>
		public List<CreateSlotCommandItem> Items { get; set; }
	}
}
