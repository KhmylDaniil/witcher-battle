using MediatR;
using Witcher.Core.Abstractions;
using Witcher.Core.Exceptions.RequestExceptions;

namespace Witcher.Core.Contracts.UserRequests.LoginUser
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
