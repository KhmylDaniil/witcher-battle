using MediatR;
using System;

namespace Witcher.Core.Contracts.UserGameRequests
{
	/// <summary>
	/// Команда удаления пользователя игры
	/// </summary>
	public sealed class DeleteUserGameCommand : IRequest
	{
		/// <summary>
		/// Айди пользователя игры
		/// </summary>
		public Guid UserId { get; set; }

		/// <summary>
		/// Имя пользователя
		/// </summary>
		public string Name { get; set; }
	}
}
