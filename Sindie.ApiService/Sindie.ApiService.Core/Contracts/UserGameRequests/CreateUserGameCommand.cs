using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Exceptions;
using System;

namespace Sindie.ApiService.Core.Contracts.UserGameRequests
{
	/// <summary>
	/// Команда создания пользователя игры
	/// </summary>
	public sealed class CreateUserGameCommand: IValidatableCommand
	{
		/// <summary>
		/// Айди пользователя
		/// </summary>
		public Guid AssignedUserId { get; set; }

		/// <summary>
		/// Айди роли в игре
		/// </summary>
		public Guid AssingedRoleId { get; set; }

		/// <summary>
		/// Валидация
		/// </summary>
		public void Validate()
		{
			if (AssingedRoleId == BaseData.GameRoles.MainMasterRoleId)
				throw new RequestValidationException("Роль главмастера невозможно присвоить");
		}
	}
}
