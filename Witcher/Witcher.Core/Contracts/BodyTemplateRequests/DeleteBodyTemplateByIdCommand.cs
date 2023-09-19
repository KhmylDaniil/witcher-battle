using System;
using MediatR;

namespace Witcher.Core.Contracts.BodyTemplateRequests
{
	/// <summary>
	/// Команда на удаление шаблона тела по айди
	/// </summary>
	public sealed class DeleteBodyTemplateByIdCommand : IRequest
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
