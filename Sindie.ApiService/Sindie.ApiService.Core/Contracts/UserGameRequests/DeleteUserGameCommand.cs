using MediatR;
using System;

namespace Sindie.ApiService.Core.Contracts.UserGameRequests
{
	/// <summary>
	/// Команда удаления пользователя игры
	/// </summary>
	public sealed class DeleteUserGameCommand : IRequest<Unit>
	{
		/// <summary>
		/// Айди игры
		/// </summary>
		public Guid GameId { get; set; }

		/// <summary>
		/// Айди пользователя игры
		/// </summary>
		public Guid UserGameId { get; set; }
	}
}
