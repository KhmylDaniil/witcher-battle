using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.UserRequests.LoginUser;
using Witcher.Core.Exceptions.RequestExceptions;

namespace Witcher.Core.Contracts.UserRequests.RegisterUser
{
	/// <summary>
	/// Запрос регистрации пользователя
	/// </summary>
	public class RegisterUserCommand : IValidatableCommand<RegisterUserCommandResponse>
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
