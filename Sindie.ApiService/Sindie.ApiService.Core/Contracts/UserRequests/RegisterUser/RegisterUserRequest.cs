using MediatR;

namespace Sindie.ApiService.Core.Contracts.UserRequests.RegisterUser
{
	/// <summary>
	/// Запрос регистрации пользователя
	/// </summary>
	public class RegisterUserRequest
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
