using System;

namespace Sindie.ApiService.Core.Contracts.UserRequests.RegisterUser
{
	/// <summary>
	/// Ответ на команду регистрации
	/// </summary>
	public class RegisterUserCommandResponse
	{
		/// <summary>
		/// Айди пользователя
		/// </summary>
		public Guid UserId { get; set; }
	}
}
