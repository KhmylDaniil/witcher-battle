using Sindie.ApiService.Core.Abstractions;
using System;

namespace Sindie.ApiService.Core.Contracts.UserGameRequests
{
	/// <summary>
	/// Команда изменения пользователя игры
	/// </summary>
	public sealed class ChangeUserGameCommand : IValidatableCommand
	{
		/// <summary>
		/// Айди интерфейса
		/// </summary>
		public Guid InterfaceId { get; set; }

		/// <summary>
		/// Валидация
		/// </summary>
		public void Validate()
		{
			// Method intentionally left empty.
		}
	}	
}
