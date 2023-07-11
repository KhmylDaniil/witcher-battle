using MediatR;
using System;

namespace Witcher.Core.Contracts.UserRequests
{
	/// <summary>
	/// Запрос регистрации пользователя
	/// </summary>
	public class RegisterUserCommand : IRequest<Guid>
	{
		/// <summary>
		/// Имя пользователя
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Емейл пользователя
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// Телефон пользователя
		/// </summary>
		public string Phone { get; set; }

		/// <summary>
		/// Логин
		/// </summary>
		public string Login { get; set; }

		/// <summary>
		/// Пароль
		/// </summary>
		public string Password { get; set; }
	}
}
