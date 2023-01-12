using MediatR;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sindie.ApiService.Core.Contracts.UserRequests.LoginUser
{
	/// <summary>
	/// Команда аутентификации пользователя
	/// </summary>
	public sealed class LoginUserCommand : IRequest<LoginUserCommandResponse>
	{
		/// <summary>
		/// Логин пользователя
		/// </summary>
		[Required]
		public string Login { get; set; }

		/// <summary>
		/// Пароль пользователя
		/// </summary>
		[Required]
		[MaxLength(25)]
		public string Password { get; set; }
	}
}
