using System;
using MediatR;

namespace Witcher.Core.Contracts.UserGameRequests
{
	/// <summary>
	/// Команда создания пользователя игры
	/// </summary>
	public class CreateUserGameCommand: IRequest
	{
		/// <summary>
		/// Айди пользователя игры
		/// </summary>
		public Guid UserId { get; set; }

		/// <summary>
		/// Айди роли в игре
		/// </summary>
		public Guid RoleId { get; set; }
	}
}
