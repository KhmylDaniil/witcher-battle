using System;
using MediatR;

namespace Witcher.Core.Contracts.CreatureTemplateRequests
{
	/// <summary>
	/// Команда на удаление шаблона существа по айди
	/// </summary>
	public sealed class DeleteCreatureTemplateByIdCommand : IRequest
	{
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
