using Witcher.Core.Abstractions;
using System;

namespace Witcher.Core.Contracts.UserGameRequests
{
	/// <summary>
	/// Команда удаления пользователя игры
	/// </summary>
	public sealed class DeleteUserGameCommand : IValidatableCommand
	{
		/// <summary>
		/// Айди пользователя игры
		/// </summary>
		public Guid UserId { get; set; }

		/// <summary>
		/// Имя пользователя
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Валидация
		/// </summary>
		public void Validate()
		{
			// Method intentionally left empty.
		}
	}
}
