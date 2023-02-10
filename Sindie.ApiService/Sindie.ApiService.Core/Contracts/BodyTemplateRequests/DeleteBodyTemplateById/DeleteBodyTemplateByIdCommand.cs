using MediatR;
using System;

namespace Sindie.ApiService.Core.Contracts.BodyTemplateRequests.DeleteBodyTemplateById
{
	/// <summary>
	/// Команда на удаление шаблона тела по айди
	/// </summary>
	public sealed class DeleteBodyTemplateByIdCommand: IRequest
	{
		/// <summary>
		/// Айди игры
		/// </summary>
		public Guid GameId { get; set; }

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
