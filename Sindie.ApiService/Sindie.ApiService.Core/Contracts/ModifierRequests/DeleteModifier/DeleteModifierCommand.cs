using MediatR;
using System;

namespace Sindie.ApiService.Core.Contracts.ModifierRequests.DeleteModifier
{
	/// <summary>
	/// Команда удаления модификатора
	/// </summary>
	public class DeleteModifierCommand: IRequest<Unit>
	{
		/// <summary>
		/// Айди игры
		/// </summary>
		public Guid GameId { get; set; }
		
		/// <summary>
		/// Айди модификатора
		/// </summary>
		public Guid Id { get; set; }
	}
}
