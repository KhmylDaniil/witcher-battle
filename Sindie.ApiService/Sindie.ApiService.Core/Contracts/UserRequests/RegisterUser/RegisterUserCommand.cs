using MediatR;
using Sindie.ApiService.Core.Contracts.UserRequests.LoginUser;
using System.ComponentModel.DataAnnotations;

namespace Sindie.ApiService.Core.Contracts.UserRequests.RegisterUser
{
	/// <summary>
	/// Запрос регистрации пользователя
	/// </summary>
	public class RegisterUserCommand : IRequest<RegisterUserCommandResponse>
	{
		/// <summary>
		/// Имя пользователя
		/// </summary>
		[Required]
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
		[Required]
		public string Login { get; set; }

		/// <summary>
		/// Пароль
		/// </summary>
		[Required]
		[MaxLength(25)]
		public string Password { get; set; }
	}
}
