﻿using System;

namespace Witcher.Core.Contracts.UserRequests.LoginUser
{
	/// <summary>
	/// Ответ на команду аутентификации
	/// </summary>
	public sealed class LoginUserCommandResponse
	{
		/// <summary>
		/// Айди пользователя
		/// </summary>
		public Guid UserId { get; set; }
	}
}
