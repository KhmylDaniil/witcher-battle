using Sindie.ApiService.Core.Abstractions;
using System;

namespace Sindie.ApiService.Core.Contracts.UserGameRequests
{
	/// <summary>
	/// Команда изменения роли пользователя игры
	/// </summary>
	public class ChangeUserGameCommand : IValidatableCommand
	{
		/// <summary>
		/// Айди пользователя игры
		/// </summary>
		public Guid UserId { get; set; }

		/// <summary>
		/// Валидация
		/// </summary>
		public void Validate()
		{
			// Method intentionally left empty.
		}
	}
}
