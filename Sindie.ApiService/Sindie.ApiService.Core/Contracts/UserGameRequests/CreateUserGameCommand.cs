using MediatR;
using System;

namespace Sindie.ApiService.Core.Contracts.UserGameRequests
{
	/// <summary>
	/// Команда создания пользователя игры
	/// </summary>
	public sealed class CreateUserGameCommand: IRequest<Unit>
	{
		/// <summary>
		/// Айди игры
		/// </summary>
		public Guid GameId { get; set; }

		/// <summary>
		/// Айди пользователя
		/// </summary>
		public Guid AssignedUserId { get; set; }

		/// <summary>
		/// Айди роли в игре
		/// </summary>
		public Guid AssingedRoleId { get; set; }
	}
}
