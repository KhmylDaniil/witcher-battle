using MediatR;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sindie.ApiService.Core.Contracts.UserRequests.LoginUser
{
	/// <summary>
	/// Команда аутентификации пользователя
	/// </summary>
	public sealed class LoginUserCommand : IValidatableCommand<LoginUserCommandResponse>
	{
		/// <summary>
		/// Логин пользователя
		/// </summary>
		public string Login { get; set; }

		/// <summary>
		/// Пароль пользователя
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		/// Валидация
		/// </summary>
		public void Validate()
		{
			if (string.IsNullOrWhiteSpace(Login))
				throw new RequestFieldNullException<LoginUserCommand>(nameof(Login));

			if (string.IsNullOrWhiteSpace(Password))
				throw new RequestFieldNullException<LoginUserCommand>(nameof(Password));

			if (Password.Length > 25)
				throw new RequestFieldIncorrectDataException<LoginUserCommand>(nameof(Password), "Превышена длина пароля");
		}
	}
}
