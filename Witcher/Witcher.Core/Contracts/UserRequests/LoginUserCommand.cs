using MediatR;
using System;
using Witcher.Core.Abstractions;
using Witcher.Core.Exceptions.RequestExceptions;

namespace Witcher.Core.Contracts.UserRequests
{
	/// <summary>
	/// Команда аутентификации пользователя
	/// </summary>
	public sealed class LoginUserCommand : IRequest<Guid>
	{
		/// <summary>
		/// Логин пользователя
		/// </summary>
		public string Login { get; set; }

		/// <summary>
		/// Пароль пользователя
		/// </summary>
		public string Password { get; set; }
	}
}
