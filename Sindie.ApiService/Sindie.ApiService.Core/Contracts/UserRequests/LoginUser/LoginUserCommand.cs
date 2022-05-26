using MediatR;

namespace Sindie.ApiService.Core.Contracts.UserRequests.LoginUser
{
	/// <summary>
	/// Команда аутентификации пользователя
	/// </summary>
	public class LoginUserCommand : IRequest<LoginUserCommandResponse>
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
