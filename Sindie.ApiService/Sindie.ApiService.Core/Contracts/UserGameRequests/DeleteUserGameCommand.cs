using Sindie.ApiService.Core.Abstractions;
using System;

namespace Sindie.ApiService.Core.Contracts.UserGameRequests
{
	/// <summary>
	/// Команда удаления пользователя игры
	/// </summary>
	public sealed class DeleteUserGameCommand : IValidatableCommand
	{
		/// <summary>
		/// Айди пользователя игры
		/// </summary>
		public Guid UserGameId { get; set; }

		/// <summary>
		/// Валидация
		/// </summary>
		public void Validate()
		{
			// Method intentionally left empty.
		}
	}
}
