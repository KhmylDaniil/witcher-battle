using MediatR;
using System;

namespace Sindie.ApiService.Core.Contracts.UserGameRequests
{
	/// <summary>
	/// Команда изменения пользователя игры
	/// </summary>
	public sealed class ChangeUserGameCommand : IRequest<Unit>
	{
		/// <summary>
		/// Айди пользователя игры
		/// </summary>
		public Guid UserGameId { get; set; }

		/// <summary>
		/// Айди интерфейса
		/// </summary>
		public Guid InterfaceId { get; set; }
	}	
}
