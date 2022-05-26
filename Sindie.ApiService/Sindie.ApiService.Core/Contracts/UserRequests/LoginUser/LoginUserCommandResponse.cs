using System;

namespace Sindie.ApiService.Core.Contracts.UserRequests.LoginUser
{
	/// <summary>
	/// Ответ на команду аутентификации
	/// </summary>
	public class LoginUserCommandResponse
	{
		/// <summary>
		/// Айди пользователя
		/// </summary>
		public Guid UserId { get; set; }

		/// <summary>
		/// Токен аутентификации
		/// </summary>
		public string AuthenticationToken { get; set; }

		/// <summary>
		/// Обновленный токен
		/// </summary>
		public string RefreshToken { get; set; }

		/// <summary>
		/// Дата создания
		/// </summary>
		public DateTime CreatedOn { get; set; }

		/// <summary>
		/// Тип токена
		/// </summary>
		public string TokenType { get; set; }
	}
}
