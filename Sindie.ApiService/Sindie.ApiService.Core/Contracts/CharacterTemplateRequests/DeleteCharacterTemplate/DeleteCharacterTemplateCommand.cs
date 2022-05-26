using MediatR;
using System;

namespace Sindie.ApiService.Core.Contracts.CharacterTemplateRequests.DeleteCharacterTemplate
{
	/// <summary>
	/// Команда удаления шаблона персонажа
	/// </summary>
	public class DeleteCharacterTemplateCommand : IRequest<Unit>
	{
		/// <summary>
		/// Айди игры
		/// </summary>
		public Guid GameId { get; set; }

		/// <summary>
		/// Айди шаблона персонажа
		/// </summary>
		public Guid Id { get; set; }
	}
}
