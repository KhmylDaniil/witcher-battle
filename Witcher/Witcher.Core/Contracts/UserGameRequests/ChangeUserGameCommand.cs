using System;
using MediatR;

namespace Witcher.Core.Contracts.UserGameRequests
{
	/// <summary>
	/// Команда изменения роли пользователя игры
	/// </summary>
	public sealed class ChangeUserGameCommand : IRequest
	{
		/// <summary>
		/// Айди пользователя игры
		/// </summary>
		public Guid UserId { get; set; }
	}
}
