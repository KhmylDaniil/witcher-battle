using Witcher.Core.Abstractions;
using Witcher.Core.Exceptions;
using System;

namespace Witcher.Core.Contracts.UserGameRequests
{
	/// <summary>
	/// Команда создания пользователя игры
	/// </summary>
	public class CreateUserGameCommand: IValidatableCommand
	{
		/// <summary>
		/// Айди пользователя игры
		/// </summary>
		public Guid UserId { get; set; }

		/// <summary>
		/// Айди роли в игре
		/// </summary>
		public Guid RoleId { get; set; }

		/// <summary>
		/// Валидация
		/// </summary>
		public void Validate()
		{
			if (RoleId == BaseData.GameRoles.MainMasterRoleId)
				throw new RequestValidationException("Роль главмастера невозможно присвоить");
		}
	}
}
